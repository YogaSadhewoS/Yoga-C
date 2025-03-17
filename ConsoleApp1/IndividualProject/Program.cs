using System;
using System.Collections.Generic;
using System.Threading;

// Enum untuk status sel pada papan
public enum CellStatus { Empty, Ship, Hit, Miss, Sunk }

// Enum untuk hasil tembakan
public enum ShotResult { Hit, Miss, Sunk, AlreadyShot, Invalid }

// Interface untuk pemain
public interface IPlayer {
    string Name { get; }
    ShotResult TakeShot(Board opponentBoard, int row, int col);
}

// Implementasi pemain manusia
public class HumanPlayer : IPlayer {
    public string Name { get; }
    public HumanPlayer(string name) { Name = name; }
    
    public ShotResult TakeShot(Board opponentBoard, int row, int col) {
        return opponentBoard.ProcessShot(row, col);
    }
}

// Interface untuk papan permainan
public interface IBoard {
    ShotResult ProcessShot(int row, int col);
    void DisplayBoard(bool hideShips);
    bool AreAllShipsSunk();
}

// Kelas papan permainan dengan ukuran 10x10
public class Board : IBoard {
    private const int Size = 10;
    private CellStatus[,] grid = new CellStatus[Size, Size];
    private List<IShip> ships = new List<IShip>();
    private Random random = new Random();

    public Board() {
        PlaceShipsRandomly();
    }

    // Menempatkan kapal secara acak sesuai ukuran standar
    private void PlaceShipsRandomly() {
        Dictionary<string, int> shipSizes = new Dictionary<string, int> {
            { "Carrier", 5 },
            { "Battleship", 4 },
            { "Cruiser", 3 },
            { "Submarine", 3 },
            { "Destroyer", 2 }
        };

        foreach (var shipEntry in shipSizes) {
            bool placed = false;
            while (!placed) {
                int row = random.Next(Size);
                int col = random.Next(Size);
                bool horizontal = random.Next(2) == 0;
                List<(int, int)> positions = new List<(int, int)>();

                for (int i = 0; i < shipEntry.Value; i++) {
                    int newRow = horizontal ? row : row + i;
                    int newCol = horizontal ? col + i : col;
                    if (newRow >= Size || newCol >= Size || grid[newRow, newCol] != CellStatus.Empty) {
                        positions.Clear();
                        break;
                    }
                    positions.Add((newRow, newCol));
                }

                if (positions.Count == shipEntry.Value) {
                    IShip ship = new Ship(positions);
                    ships.Add(ship);
                    foreach (var pos in positions)
                        grid[pos.Item1, pos.Item2] = CellStatus.Ship;
                    placed = true;
                }
            }
        }
    }

    // Memproses tembakan pada koordinat (row, col)
    public ShotResult ProcessShot(int row, int col) {
        if (row < 0 || row >= Size || col < 0 || col >= Size)
            return ShotResult.Invalid;
        if (grid[row, col] == CellStatus.Hit || grid[row, col] == CellStatus.Miss || grid[row, col] == CellStatus.Sunk)
            return ShotResult.AlreadyShot;

        if (grid[row, col] == CellStatus.Ship) {
            foreach (IShip ship in ships) {
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
        }

        grid[row, col] = CellStatus.Miss;
        return ShotResult.Miss;
    }

    public bool AreAllShipsSunk() {
        foreach (var ship in ships)
            if (!ship.IsSunk())
                return false;
        return true;
    }

    // Menampilkan papan dengan pewarnaan:
    // Hit (O) = Biru, Miss (X) = Merah, Sunk (S) = Hijau.
    public void DisplayBoard(bool hideShips) {
        Console.Clear();
        Console.Write("    ");
        for (int j = 0; j < Size; j++) {
            Console.Write(j.ToString().PadLeft(2) + " ");
        }
        Console.WriteLine();
        for (int i = 0; i < Size; i++) {
            Console.Write(i.ToString().PadLeft(2) + "  ");
            for (int j = 0; j < Size; j++) {
                string symbol = "|";
                ConsoleColor color = Console.ForegroundColor;

                if (grid[i, j] == CellStatus.Hit) { symbol = "O"; color = ConsoleColor.Blue; }
                else if (grid[i, j] == CellStatus.Miss) { symbol = "X"; color = ConsoleColor.Red; }
                else if (grid[i, j] == CellStatus.Sunk) { symbol = "S"; color = ConsoleColor.Green; }
                else if (!hideShips && grid[i, j] == CellStatus.Ship) { symbol = "S"; color = ConsoleColor.Green; }

                Console.ForegroundColor = color;
                Console.Write(symbol.PadLeft(2) + " ");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}

// Interface untuk kapal
public interface IShip {
    bool RecordHit(int row, int col);
    bool IsSunk();
    bool IsPositionPartOfShip(int row, int col);
    List<(int, int)> GetOccupiedPositions();
}

// Implementasi kapal sederhana
public class Ship : IShip {
    private List<(int, int)> positions;
    private HashSet<(int, int)> hits = new HashSet<(int, int)>();

    public Ship(List<(int, int)> positions) {
        this.positions = positions;
    }

    public bool RecordHit(int row, int col) {
        if (positions.Contains((row, col))) {
            hits.Add((row, col));
            return true;
        }
        return false;
    }

    public bool IsSunk() {
        return hits.Count == positions.Count;
    }

    public bool IsPositionPartOfShip(int row, int col) {
        return positions.Contains((row, col));
    }

    public List<(int, int)> GetOccupiedPositions() {
        return positions;
    }
}

// Kelas pengontrol permainan untuk mode Versus Friend
public class GameController {
    private IPlayer player1, player2;
    private IBoard boardPlayer1, boardPlayer2;

    public GameController() {
        Console.Write("Masukkan nama Player 1 (default: Player 1): ");
        string name1 = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name1))
            name1 = "Player 1";

        Console.Write("Masukkan nama Player 2 (default: Player 2): ");
        string name2 = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name2))
            name2 = "Player 2";

        player1 = new HumanPlayer(name1);
        player2 = new HumanPlayer(name2);
        boardPlayer1 = new Board();
        boardPlayer2 = new Board();
    }

    public void StartGame() {
        bool player1Turn = true;
        while (true) {
            IPlayer currentPlayer = player1Turn ? player1 : player2;
            IBoard opponentBoard = player1Turn ? boardPlayer2 : boardPlayer1;
            
            // Tampilkan papan lawan (tanpa menampilkan kapal)
            opponentBoard.DisplayBoard(true);
            Console.WriteLine($"\nGiliran {currentPlayer.Name} - Tebak papan lawan:");
            Console.Write("\nMasukkan koordinat (row,col): ");
            string input = Console.ReadLine();
            
            if (TryParseCoordinates(input, out int row, out int col)) {
                ShotResult result = currentPlayer.TakeShot((Board)opponentBoard, row, col);
                
                if(result == ShotResult.AlreadyShot) {
                    Console.WriteLine("\nSel sudah ditembak, silahkan masukkan koordinat lain. Tekan Enter untuk mencoba lagi.");
                    Console.ReadLine();
                    continue;
                }
                
                string message = "";
                if (result == ShotResult.Hit)
                    message = "Tembakan mengenai kapal!";
                else if (result == ShotResult.Miss)
                    message = "Tembakan meleset!";
                else if (result == ShotResult.Sunk)
                    message = "Kapal hancur!";

                opponentBoard.DisplayBoard(true);
                Console.WriteLine($"\n{currentPlayer.Name} menembak -> {result}");
                Console.WriteLine(message);

                if (opponentBoard.AreAllShipsSunk()) {
                    Console.WriteLine($"\n{currentPlayer.Name} MENANG!");
                    break;
                }
                Thread.Sleep(2000);
            } else {
                Console.WriteLine("\nInput tidak valid. Tekan Enter untuk mencoba lagi.");
                Console.ReadLine();
                continue;
            }
            player1Turn = !player1Turn;
        }
        Console.WriteLine("\nPermainan selesai. Tekan sembarang tombol untuk keluar...");
        Console.ReadKey();
    }

    private bool TryParseCoordinates(string input, out int row, out int col) {
        row = col = 0;
        string[] parts = input.Split(',');
        return parts.Length == 2 &&
               int.TryParse(parts[0].Trim(), out row) &&
               int.TryParse(parts[1].Trim(), out col);
    }
}

public class Program {
    public static void Main() {
        Console.WriteLine("Selamat datang di Battleship Versus!");
        GameController controller = new GameController();
        controller.StartGame();
    }
}
