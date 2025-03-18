using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BattleshipGame
{
    // === ENUMERASI ===
    public enum ShipType
    {
        CARRIER = 5,
        BATTLESHIP = 4,
        CRUISER = 3,
        SUBMARINE = 3,
        DESTROYER = 2
    }

    public enum CellStatus
    {
        EMPTY,
        SHIP,
        HIT,
        MISS,
        SUNK //Ditambah untuk keperluan visual
    }

    public enum Orientation
    {
        HORIZONTAL,
        VERTICAL
    }

    public enum GameState
    {
        SETUP,
        PLAYING,
        FINISHED
    }

    public enum ShotResult
    {
        HIT,
        MISS,
        SUNK,
        INVALID,
        ALREADY_SHOT
    }

    // === INTERFACE IPLAYER DAN IMPLEMENTASI PLAYER ===
    public interface IPlayer
    {
        string Name { get; }
        void UpdateStatistic(string gameType, string stat, int value);
        int GetStatistic(string gameType, string stat);
    }

    public class Player : IPlayer
    {
        private string name;
        private string id;
        private Dictionary<string, Dictionary<string, int>> statistics;

        public string Name => name;
        // public string Id => id; //Menyimpan id pemain di luar game

        public Player(string name)
        {
            this.name = name;
            this.id = Guid.NewGuid().ToString();
            statistics = new Dictionary<string, Dictionary<string, int>>();
        }
        
        public void UpdateStatistic(string gameType, string stat, int value)
        {
            if (!statistics.ContainsKey(gameType))
                statistics[gameType] = new Dictionary<string, int>();
            if (!statistics[gameType].ContainsKey(stat))
                statistics[gameType][stat] = 0;
            statistics[gameType][stat] += value;
        }

        public int GetStatistic(string gameType, string stat)
        {
            if (statistics.ContainsKey(gameType) && statistics[gameType].ContainsKey(stat))
                return statistics[gameType][stat];
            return 0;
        }
    }

    // === INTERFACE ISHIP DAN IMPLEMENTASI SHIP ===
    public interface IShip
    {
        bool RecordHit(int row, int column);
        bool IsSunk();
        bool IsPositionPartOfShip(int row, int column);
        (int, int)[] GetOccupiedPositions();
    }

    public class Ship : IShip
    {
        private ShipType type;
        private int startRow;
        private int startColumn;
        private Orientation orientation;
        private bool[] hitArray;

        // public ShipType Type => type;
        public int Size => (int)type;

        public Ship(ShipType type)
        {
            this.type = type;
            hitArray = new bool[(int)type];
        }

        // Menempatkan kapal pada koordinat (row, column) dengan orientasi tertentu
        public void PlaceAt(int row, int column, Orientation orientation)
        {
            this.startRow = row;
            this.startColumn = column;
            this.orientation = orientation;
        }

        public (int, int)[] GetOccupiedPositions()
        {
            var positions = new (int, int)[Size];
            for (int i = 0; i < Size; i++)
            {
                int r = startRow + (orientation == Orientation.VERTICAL ? i : 0);
                int c = startColumn + (orientation == Orientation.HORIZONTAL ? i : 0);
                positions[i] = (r, c);
            }
            return positions;
        }

        public bool IsPositionPartOfShip(int row, int column)
        {
            return GetOccupiedPositions().Any(pos => pos.Item1 == row && pos.Item2 == column);
        }

        public bool RecordHit(int row, int column)
        {
            var positions = GetOccupiedPositions();
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i].Item1 == row && positions[i].Item2 == column)
                {
                    hitArray[i] = true;
                    return true;
                }
            }
            return false;
        }

        public bool IsSunk()
        {
            return hitArray.All(hit => hit);
        }
    }

    // === INTERFACE IBOARD DAN IMPLEMENTASI BOARD ===
    public interface IBoard
    {
        bool PlaceShip(IShip ship, int row, int column, Orientation orientation);
        CellStatus GetCellStatus(int row, int column);
        void SetCellStatus(int row, int column, CellStatus status);
        IShip GetShipAt(int row, int column);
        bool IsPositionValid(int row, int column);
        IReadOnlyList<IShip> GetAllShips();
        void DisplayBoard(bool hideShips);
    }

    public class Board : IBoard
    {
        private int size;
        private CellStatus[,] cellStatus;
        private IShip[,] cellShips;
        private List<IShip> ships;

        public int Size => size;

        public Board(int size)
        {
            this.size = size;
            cellStatus = new CellStatus[size, size];
            cellShips = new IShip[size, size];
            ships = new List<IShip>();

            // Inisialisasi semua sel sebagai EMPTY
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    cellStatus[i, j] = CellStatus.EMPTY;
        }

        public bool IsPositionValid(int row, int column)
        {
            return row >= 0 && row < size && column >= 0 && column < size;
        }

        public bool PlaceShip(IShip ship, int row, int column, Orientation orientation)
        {
            // Cast ship untuk menggunakan method PlaceAt
            Ship s = ship as Ship;
            if (s == null)
                return false;

            s.PlaceAt(row, column, orientation);
            var positions = s.GetOccupiedPositions();

            // Validasi setiap posisi
            foreach (var pos in positions)
            {
                if (!IsPositionValid(pos.Item1, pos.Item2) || cellStatus[pos.Item1, pos.Item2] != CellStatus.EMPTY)
                    return false;
            }

            // Tempatkan kapal pada papan
            foreach (var pos in positions)
            {
                cellStatus[pos.Item1, pos.Item2] = CellStatus.SHIP;
                cellShips[pos.Item1, pos.Item2] = ship;
            }
            ships.Add(ship);
            return true;
        }

        public CellStatus GetCellStatus(int row, int column)
        {
            if (!IsPositionValid(row, column))
                throw new ArgumentException("Invalid position");
            return cellStatus[row, column];
        }

        public void SetCellStatus(int row, int column, CellStatus status)
        {
            if (IsPositionValid(row, column))
                cellStatus[row, column] = status;
        }

        public IShip GetShipAt(int row, int column)
        {
            if (IsPositionValid(row, column))
                return cellShips[row, column];
            return null;
        }

        public IReadOnlyList<IShip> GetAllShips()
        {
            return ships.AsReadOnly();
        }

        // Tampilan papan dengan pewarnaan:
        // HIT (O) = Biru, MISS (X) = Merah, dan jika tidak disembunyikan, SHIP (S) = Hijau.
        public void DisplayBoard(bool hideShips)
        {
            Console.Clear();
            Console.Write("    ");
            for (int j = 0; j < size; j++)
                Console.Write(j.ToString().PadLeft(2) + " ");
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                Console.Write(i.ToString().PadLeft(2) + "  ");
                for (int j = 0; j < size; j++)
                {
                    string symbol = "|";
                    ConsoleColor color = Console.ForegroundColor;

                    if (cellStatus[i, j] == CellStatus.HIT) { symbol = "O"; color = ConsoleColor.Blue; }
                    else if (cellStatus[i, j] == CellStatus.MISS) { symbol = "X"; color = ConsoleColor.Red; }
                    else if (cellStatus[i, j] == CellStatus.SUNK) { symbol = "S"; color = ConsoleColor.Green; }
                    else if (!hideShips && cellStatus[i, j] == CellStatus.SHIP) { symbol = "|";}


                    Console.ForegroundColor = color;
                    Console.Write(symbol.PadLeft(2) + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }

    // === SHIP FACTORY ===
    public class ShipFactory
    {
        public IShip CreateShip(ShipType type)
        {
            return new Ship(type);
        }

        public List<IShip> CreateStandardShipSet()
        {
            return new List<IShip>
            {
                CreateShip(ShipType.CARRIER),
                CreateShip(ShipType.BATTLESHIP),
                CreateShip(ShipType.CRUISER),
                CreateShip(ShipType.SUBMARINE),
                CreateShip(ShipType.DESTROYER)
            };
        }
    }

    // === GAME CONTROLLER ===
    public class GameController
    {
        private Dictionary<IPlayer, IBoard> boards;
        private Dictionary<IPlayer, List<IShip>> playerShips;
        private List<IPlayer> playerOrder;
        private int currentPlayerIndex;
        private GameState state;

        public event Action<IPlayer> OnTurnChanged;
        public event Action<IPlayer> OnGameOver;
        public event Action<IShip> OnShipSunk;
        public event Action<int, int, ShotResult> OnShotProcessed;

        private int boardSize;

        public GameController()
        {
            boards = new Dictionary<IPlayer, IBoard>();
            playerShips = new Dictionary<IPlayer, List<IShip>>();
            playerOrder = new List<IPlayer>();
            currentPlayerIndex = 0;
            state = GameState.SETUP;
        }

        public void InitializeGame(int boardSize, int playerCount)
        {
            this.boardSize = boardSize;
            // Misal untuk permainan 2 pemain
            for (int i = 0; i < playerCount; i++)
            {
                Console.Write($"Masukkan nama Player {i + 1} (default: Player {i + 1}): ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    name = $"Player {i + 1}";
                IPlayer player = new Player(name);
                playerOrder.Add(player);
                boards[player] = new Board(boardSize);
                playerShips[player] = new List<IShip>();

                // Penempatan kapal secara acak menggunakan ShipFactory
                ShipFactory factory = new ShipFactory();
                List<IShip> ships = factory.CreateStandardShipSet();
                Random rand = new Random();
                foreach (IShip ship in ships)
                {
                    bool placed = false;
                    while (!placed)
                    {
                        int row = rand.Next(boardSize);
                        int col = rand.Next(boardSize);
                        Orientation orientation = (rand.Next(2) == 0) ? Orientation.HORIZONTAL : Orientation.VERTICAL;
                        if (boards[player].PlaceShip(ship, row, col, orientation))
                        {
                            placed = true;
                            playerShips[player].Add(ship);
                        }
                    }
                }
            }
            state = GameState.PLAYING;
        }

        public bool StartGame()
        {
            while (!IsGameOver())
            {
                IPlayer currentPlayer = GetCurrentPlayer();
                IPlayer opponent = playerOrder[(currentPlayerIndex + 1) % playerOrder.Count];
                IBoard opponentBoard = boards[opponent];

                // Tampilkan papan lawan sebelum tembakan
                opponentBoard.DisplayBoard(true);
                Console.WriteLine($"\nGiliran {currentPlayer.Name}. Masukkan koordinat tembakan (row,col): ");
                string input = Console.ReadLine();
                var parts = input.Split(',');
                if (parts.Length != 2 ||
                    !int.TryParse(parts[0].Trim(), out int row) ||
                    !int.TryParse(parts[1].Trim(), out int col))
                {
                    Console.WriteLine("Input tidak valid. Tekan Enter untuk mencoba lagi.");
                    Console.ReadLine();
                    continue;
                }
                ShotResult result = ProcessShot(row, col);
                OnShotProcessed?.Invoke(row, col, result);

                if (result == ShotResult.ALREADY_SHOT)
                {
                    Console.WriteLine("Sudah ditembak, silakan masukkan koordinat lain. Tekan Enter untuk mencoba lagi.");
                    Console.ReadLine();
                    continue;
                }

                // Tampilkan papan yang sudah diperbarui setelah tembakan
                opponentBoard.DisplayBoard(true);
                Console.WriteLine($"{currentPlayer.Name} menembak ke ({row},{col}) -> {result}");

                if (result == ShotResult.SUNK)
                {
                    OnShipSunk?.Invoke(null); // Pemicu event untuk kapal tenggelam (contoh)
                }
                if (IsGameOver())
                {
                    Console.WriteLine($"\n{currentPlayer.Name} MENANG!");
                    OnGameOver?.Invoke(currentPlayer);
                    state = GameState.FINISHED;
                    return true;
                }
                Thread.Sleep(2000);
                SwitchTurn();
            }
            return false;
        }

        //Untuk menambahkan fitur kapal secara manual, penempatan sudah otomatis di InitializeGame()
        public bool PlaceShip(IPlayer player, ShipType type, int row, int column, Orientation orientation)
        {
            IBoard board = boards[player];
            IShip ship = new Ship(type);
            if (board.PlaceShip(ship, row, column, orientation))
            {
                playerShips[player].Add(ship);
                return true;
            }
            return false;
        }

        public ShotResult ProcessShot(int row, int column)
        {
            IPlayer currentPlayer = playerOrder[currentPlayerIndex]; // Pemain yang sedang menembak
            IPlayer opponent = playerOrder[(currentPlayerIndex + 1) % playerOrder.Count]; // Lawan yang ditembak
            IBoard opponentBoard = boards[opponent]; // Papan lawan

            if (!opponentBoard.IsPositionValid(row, column))
                return ShotResult.INVALID;

            CellStatus cellStatus = opponentBoard.GetCellStatus(row, column);

            if (cellStatus == CellStatus.HIT || cellStatus == CellStatus.MISS || cellStatus == CellStatus.SUNK)
                return ShotResult.ALREADY_SHOT;

            if (cellStatus == CellStatus.SHIP)
            {
                IShip ship = opponentBoard.GetShipAt(row, column);
                if (ship.RecordHit(row, column))
                {
                    opponentBoard.SetCellStatus(row, column, CellStatus.HIT);
                    if (ship.IsSunk())
                    {
                        foreach (var pos in ship.GetOccupiedPositions())
                            opponentBoard.SetCellStatus(pos.Item1, pos.Item2, CellStatus.SUNK);
                        return ShotResult.SUNK;
                    }
                    return ShotResult.HIT;
                }
            }

            opponentBoard.SetCellStatus(row, column, CellStatus.MISS);
            return ShotResult.MISS;
        }



        public void SwitchTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playerOrder.Count;
            OnTurnChanged?.Invoke(GetCurrentPlayer());
        }

        public bool IsGameOver()
        {
            // Permainan selesai jika semua kapal lawan telah hancur
            IPlayer opponent = playerOrder[(currentPlayerIndex + 1) % playerOrder.Count];
            foreach (IShip ship in boards[opponent].GetAllShips())
            {
                if (!ship.IsSunk())
                    return false;
            }
            return true;
        }

        public IPlayer GetCurrentPlayer()
        {
            return playerOrder[currentPlayerIndex];
        }

        //Tidak mengembalikan papan pemain tertentu
        public IBoard GetPlayerBoard(IPlayer player)
        {
            return boards[player];
        }

        //Tidak mengembalikan daftar kapal milik pemain
        public IReadOnlyList<IShip> GetPlayerShips(IPlayer player)
        {
            return playerShips[player].AsReadOnly();
        }
    }

    // === GAME UI ===
    public class GameUI
    {
        private GameController controller;

        public GameUI(GameController controller)
        {
            this.controller = controller;
        }

        //DisplayBoard() sudah menampilkan status papan & giliran pemain
        public void RenderGameState(Dictionary<IPlayer, IBoard> boards, IPlayer currentPlayer)
        {
            Console.WriteLine($"Giliran: {currentPlayer.Name}");
            foreach (var kvp in boards)
            {
                Console.WriteLine($"Papan {kvp.Key.Name}:");
                kvp.Value.DisplayBoard(true);
            }
        }

        //Tidak menampilkan papan milik pemain tertentu
        public void RenderPlayerBoard(IPlayer player, IBoard board, bool hideShips)
        {
            Console.WriteLine($"Papan {player.Name}:");
            board.DisplayBoard(hideShips);
        }

        //Sudah diatur GameController dalam menampilkan pesan
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        //Jika nantinya ingin memasukkan kapal secara manual
        public (ShipType, int, int, Orientation) GetPlacementInput()
        {
            Console.WriteLine("Masukkan jenis kapal (0: CARRIER, 1: BATTLESHIP, 2: CRUISER, 3: SUBMARINE, 4: DESTROYER): ");
            int typeInput = int.Parse(Console.ReadLine());
            ShipType type = (ShipType)Enum.GetValues(typeof(ShipType)).GetValue(typeInput);

            Console.WriteLine("Masukkan baris penempatan: ");
            int row = int.Parse(Console.ReadLine());
            Console.WriteLine("Masukkan kolom penempatan: ");
            int col = int.Parse(Console.ReadLine());
            Console.WriteLine("Masukkan orientasi (0: HORIZONTAL, 1: VERTICAL): ");
            int oriInput = int.Parse(Console.ReadLine());
            Orientation orientation = (Orientation)oriInput;

            return (type, row, col, orientation);
        }

        //Sudah ditangani di StartGame()
        public (int, int) GetShotInput()
        {
            Console.WriteLine("Masukkan koordinat tembakan (row,col): ");
            string input = Console.ReadLine();
            var parts = input.Split(',');
            int row = int.Parse(parts[0].Trim());
            int col = int.Parse(parts[1].Trim());
            return (row, col);
        }

        public void StartGameLoop()
        {
            controller.InitializeGame(10, 2);
            controller.StartGame();
        }
    }

    // === PROGRAM UTAMA ===
    public class Program
    {
        public static void Main(string[] args)
        {
            GameController game = new GameController();
            GameUI ui = new GameUI(game);
            ui.StartGameLoop();
            Console.WriteLine("Tekan sembarang tombol untuk keluar...");
            Console.ReadKey();
        }
    }
}
