using System;
using System.Collections.Generic;

// Enum untuk status sel
public enum CellStatus { Empty, Ship, Hit, Miss, Sunk }

// Enum untuk hasil tembakan
public enum ShotResult { Hit, Miss, Sunk, Invalid, AlreadyShot }

// Enum untuk tipe kapal
public enum ShipType { Carrier, Battleship, Cruiser, Submarine, Destroyer }

// Interface untuk kapal
public interface IShip {
    bool RecordHit(int row, int col);
    bool IsSunk();
    bool IsPositionPartOfShip(int row, int col);
    List<(int, int)> GetOccupiedPositions();
}

// Implementasi kapal
public class Ship : IShip {
    private List<(int, int)> positions;
    private HashSet<(int, int)> hits;
    
    public Ship(List<(int, int)> positions) {
        this.positions = positions;
        this.hits = new HashSet<(int, int)>();
    }
    
    public bool RecordHit(int row, int col) {
        if (positions.Contains((row, col))) {
            hits.Add((row, col));
            return true;
        }
        return false;
    }
    
    public bool IsSunk() => hits.Count == positions.Count;
    
    public bool IsPositionPartOfShip(int row, int col) => positions.Contains((row, col));
    
    public List<(int, int)> GetOccupiedPositions() => positions;
}

// Interface untuk papan permainan
public interface IBoard {
    ShotResult ProcessShot(int row, int col);
    void DisplayBoard(bool revealShips);
    bool AreAllShipsSunk();
}

// Implementasi papan permainan
public class Board : IBoard {
    private const int Size = 10;
    private CellStatus[,] grid = new CellStatus[Size, Size];
    private List<IShip> ships = new List<IShip>();
    private Random random = new Random();
    
    public Board() {
        PlaceShipsRandomly();
    }

    private void PlaceShipsRandomly() {
        Dictionary<ShipType, int> shipSizes = new Dictionary<ShipType, int> {
            { ShipType.Carrier, 5 },
            { ShipType.Battleship, 4 },
            { ShipType.Cruiser, 3 },
            { ShipType.Submarine, 3 },
            { ShipType.Destroyer, 2 }
        };

        foreach (var shipType in shipSizes) {
            bool placed = false;
            while (!placed) {
                int row = random.Next(Size);
                int col = random.Next(Size);
                bool horizontal = random.Next(2) == 0;

                List<(int, int)> positions = new List<(int, int)>();
                for (int i = 0; i < shipType.Value; i++) {
                    int newRow = horizontal ? row : row + i;
                    int newCol = horizontal ? col + i : col;

                    if (newRow >= Size || newCol >= Size || grid[newRow, newCol] != CellStatus.Empty) {
                        positions.Clear();
                        break;
                    }
                    positions.Add((newRow, newCol));
                }

                if (positions.Count == shipType.Value) {
                    ships.Add(new Ship(positions));
                    foreach (var pos in positions)
                        grid[pos.Item1, pos.Item2] = CellStatus.Ship;
                    placed = true;
                }
            }
        }
    }
    
    public ShotResult ProcessShot(int row, int col) {
        if (row < 0 || row >= Size || col < 0 || col >= Size)
            return ShotResult.Invalid;
        if (grid[row, col] == CellStatus.Hit || grid[row, col] == CellStatus.Miss)
            return ShotResult.AlreadyShot;
        
        foreach (var ship in ships) {
            if (ship.IsPositionPartOfShip(row, col)) {
                ship.RecordHit(row, col);
                if (ship.IsSunk()) {
                    foreach (var pos in ship.GetOccupiedPositions())
                        grid[pos.Item1, pos.Item2] = CellStatus.Sunk;
                    return ShotResult.Sunk;
                }
                grid[row, col] = CellStatus.Hit;
                return ShotResult.Hit;
            }
        }
        
        grid[row, col] = CellStatus.Miss;
        return ShotResult.Miss;
    }
    
    public void DisplayBoard(bool revealShips) {
        Console.Clear(); // Membersihkan layar sebelum menggambar ulang board
        Console.Write("   ");
        for (int j = 0; j < Size; j++) Console.Write(j + " ");
        Console.WriteLine();
        
        for (int i = 0; i < Size; i++) {
            Console.Write(i + "  ");
            for (int j = 0; j < Size; j++) {
                if (grid[i, j] == CellStatus.Hit) Console.Write("O ");
                else if (grid[i, j] == CellStatus.Miss) Console.Write("X ");
                else if (grid[i, j] == CellStatus.Sunk) Console.Write("S ");
                else if (revealShips && grid[i, j] == CellStatus.Ship) Console.Write("S ");
                else Console.Write("| ");
            }
            Console.WriteLine();
        }
    }
    
    public bool AreAllShipsSunk() => ships.TrueForAll(s => s.IsSunk());
}

// Kelas utama untuk permainan
public class GameController {
    private IBoard board;
    public GameController() {
        board = new Board();
    }
    
    public void StartGame() {
        Console.WriteLine("Selamat datang di Battleship!");
        while (!board.AreAllShipsSunk()) {
            board.DisplayBoard(false);
            Console.Write("Masukkan koordinat (baris,kolom): ");
            var input = Console.ReadLine().Split(',');
            if (input.Length != 2 || !int.TryParse(input[0], out int row) || !int.TryParse(input[1], out int col)) {
                Console.WriteLine("Input tidak valid!");
                continue;
            }
            
            var result = board.ProcessShot(row, col);
            Console.WriteLine(result);
            System.Threading.Thread.Sleep(1000); // Delay agar pemain sempat membaca output sebelum layar dibersihkan
        }
        Console.WriteLine("Selamat! Semua kapal telah dihancurkan.");
        board.DisplayBoard(true);
    }
}

// Entry point program
public class Program {
    public static void Main(string[] args) {
        new GameController().StartGame();
    }
}
