using System;
using System.Collections.Generic;
using System.Threading;

// Enum untuk status sel pada papan
public enum CellStatus { Empty, Ship, Hit, Miss, Sunk }

// Enum untuk hasil tembakan
public enum ShotResult { Hit, Miss, Sunk, AlreadyShot, Invalid }

// Enum untuk mode permainan
public enum GameMode { SinglePlayer, VersusFriend }

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

    // Menempatkan kapal secara acak berdasarkan ukuran standar
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
        for (int i = 0; i < Size; i++) {
            for (int j = 0; j < Size; j++) {
                if (grid[i, j] == CellStatus.Ship)
                    return false;
            }
        }
        return true;
    }

    // Menampilkan papan (menggunakan Console.Clear agar board lama tertimpa)
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
                string symbol = "";
                if (grid[i, j] == CellStatus.Hit)
                    symbol = "O";
                else if (grid[i, j] == CellStatus.Miss)
                    symbol = "X";
                else if (grid[i, j] == CellStatus.Sunk)
                    symbol = "S";
                else if (!hideShips && grid[i, j] == CellStatus.Ship)
                    symbol = "S";
                else
                    symbol = "|";
                Console.Write(symbol.PadLeft(2) + " ");
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

// Kelas pengontrol permainan
public class GameController {
    private GameMode mode;
    private IPlayer player1, player2;
    private IBoard boardTarget;             // Untuk mode SinglePlayer
    private IBoard boardPlayer1, boardPlayer2; // Untuk mode VersusFriend

    public GameController(GameMode mode) {
        this.mode = mode;
        if (mode == GameMode.SinglePlayer) {
            player1 = new HumanPlayer("Player");
            boardTarget = new Board();
        } else { // VersusFriend
            // Console.Write("Masukkan nama Player 1: ");
            // string name1 = Console.ReadLine();
            // Console.Write("Masukkan nama Player 2: ");
            // string name2 = Console.ReadLine();
            // player1 = new HumanPlayer(name1);
            // player2 = new HumanPlayer(name2);
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

            boardPlayer1 = new Board(); // Papan milik Player 1 (ditebak oleh Player 2)
            boardPlayer2 = new Board(); // Papan milik Player 2 (ditebak oleh Player 1)
        }
    }

    public void StartGame() {
        if (mode == GameMode.SinglePlayer)
            RunSinglePlayerGame();
        else
            RunVersusFriendGame();
    }

    // Mode bermain sendiri
    private void RunSinglePlayerGame() {
        while (true) {
            boardTarget.DisplayBoard(true);
            Console.Write("\nMasukkan koordinat (row,col): ");
            if (TryParseCoordinates(Console.ReadLine(), out int row, out int col)) {
                ShotResult result = player1.TakeShot((Board)boardTarget, row, col);
                string message = "";
                if (result == ShotResult.Hit)
                    message = "Tembakan mengenai kapal!";
                else if (result == ShotResult.Miss)
                    message = "Tembakan meleset!";
                else if (result == ShotResult.Sunk)
                    message = "Kapal hancur!";
                
                // Tampilkan board dan pesan bersama
                boardTarget.DisplayBoard(true);
                Console.WriteLine($"\nHasil tembakan: {result}");
                Console.WriteLine(message);
                
                if (boardTarget.AreAllShipsSunk()) {
                    Console.WriteLine("\nSelamat, Anda telah menghancurkan semua kapal!");
                    break;
                }
            } else {
                Console.WriteLine("\nInput tidak valid. Tekan Enter untuk mencoba lagi.");
                Console.ReadLine();
            }
            Thread.Sleep(1500);
        }
    }

    // Mode bermain dengan teman
    private void RunVersusFriendGame() {
        bool player1Turn = true;
        while (true) {
            IPlayer currentPlayer = player1Turn ? player1 : player2;
            IBoard opponentBoard = player1Turn ? boardPlayer2 : boardPlayer1;
            // Tampilkan papan lawan dan informasi giliran
            opponentBoard.DisplayBoard(true);
            Console.WriteLine($"\nGiliran {currentPlayer.Name} - Tebak papan lawan:");
            Console.Write("\nMasukkan koordinat (row,col): ");
            
            if (TryParseCoordinates(Console.ReadLine(), out int row, out int col)) {
                ShotResult result = currentPlayer.TakeShot((Board)opponentBoard, row, col);
                string message = "";
                if (result == ShotResult.Hit)
                    message = "Tembakan mengenai kapal!";
                else if (result == ShotResult.Miss)
                    message = "Tembakan meleset!";
                else if (result == ShotResult.Sunk)
                    message = "Kapal hancur!";
                
                // Tampilkan board dan pesan bersama
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
    }

    private bool TryParseCoordinates(string input, out int row, out int col) {
        row = col = 0;
        string[] parts = input.Split(',');
        return parts.Length == 2 &&
               int.TryParse(parts[0].Trim(), out row) &&
               int.TryParse(parts[1].Trim(), out col);
    }
}

// Entry point program
public class Program {
    public static void Main() {
        Console.WriteLine("Selamat datang di Battleship!");
        Console.WriteLine("Pilih mode permainan:");
        Console.WriteLine("1. Bermain Sendiri");
        Console.WriteLine("2. Bermain dengan Teman");
        Console.Write("Pilihan: ");
        string input = Console.ReadLine();
        GameMode mode = input == "2" ? GameMode.VersusFriend : GameMode.SinglePlayer;
        GameController controller = new GameController(mode);
        controller.StartGame();
    }
}