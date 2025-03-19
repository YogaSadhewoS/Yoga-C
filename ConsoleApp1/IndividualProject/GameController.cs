namespace BattleshipGame
{
    public class GameController
    {
        private Dictionary<IPlayer, IBoard> boards;
        private Dictionary<IPlayer, List<IShip>> playerShips;
        private List<IPlayer> playerOrder;
        private int currentPlayerIndex;
        private GameState state;

        public event Action<IPlayer>? OnTurnChanged;
        public event Action<IPlayer>? OnGameOver;
        public event Action<IShip>? OnShipSunk;
        public event Action<int, int, ShotResult>? OnShotProcessed;

        private int boardSize;

        //Menginisialisasi koleksi internal dan menetapkan state awal permainan (SETUP)
        public GameController()
        {
            boards = new Dictionary<IPlayer, IBoard>();
            playerShips = new Dictionary<IPlayer, List<IShip>>();
            playerOrder = new List<IPlayer>();
            currentPlayerIndex = 0;
            state = GameState.SETUP;
        }

        //Menyiapkan permainan dengan membuat pemain, papan, dan menempatkan kapal secara acak
        public void InitializeGame(int boardSize, int playerCount)
        {
            this.boardSize = boardSize;
            // Untuk permainan 2 pemain
            for (int i = 0; i < playerCount; i++)
            {
                Console.Write($"Masukkan nama Player {i + 1} (default: Player {i + 1}): ");
                string? name = Console.ReadLine();
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

        //Memulai dan mengelola loop utama permainan hingga kondisi game over tercapai
        public bool StartGame()
        {
            while (!IsGameOver())
            {
                IPlayer currentPlayer = GetCurrentPlayer();
                IPlayer opponent = playerOrder[(currentPlayerIndex + 1) % playerOrder.Count];
                IBoard opponentBoard = boards[opponent];
                IBoardDisplay boardDisplay = new BoardDisplay();

                // Tampilkan papan lawan sebelum tembakan
                boardDisplay.RenderBoard(opponentBoard, true);
                Console.WriteLine($"\nGiliran {currentPlayer.Name}. Masukkan koordinat tembakan (misal A5): ");
                string? rawInput = Console.ReadLine();
                if (string.IsNullOrEmpty(rawInput))
                {
                    Console.WriteLine("Input tidak valid. Tekan Enter untuk mencoba lagi.");
                    Console.ReadLine();
                    continue;
                }
                string input = rawInput.Trim().ToUpper(); // Ubah ke huruf kapital agar "a5" tetap diterima


                if (input.Length < 2 || !char.IsLetter(input[0]) || !char.IsDigit(input[1]))
                {
                    Console.WriteLine("Input tidak valid. Tekan Enter untuk mencoba lagi.");
                    Console.ReadLine();
                    continue;
                }

                int col = input[0] - 'A'; // Konversi huruf ke indeks kolom (A = 0, B = 1, dst.)
                if (!int.TryParse(input.Substring(1), out int row) || row < 1 || row > boardSize)
                {
                    Console.WriteLine("Input tidak valid. Tekan Enter untuk mencoba lagi.");
                    Console.ReadLine();
                    continue;
                }

                row -= 1; // Sesuaikan dengan indeks array (baris mulai dari 1, tapi indeks array 0-based)

                ShotResult result = ProcessShot(row, col);
                OnShotProcessed?.Invoke(row, col, result);

                if (result == ShotResult.ALREADY_SHOT)
                {
                    Console.WriteLine("Sudah ditembak, silakan masukkan koordinat lain. Tekan Enter untuk mencoba lagi.");
                    Console.ReadLine();
                    continue;
                }

                // Tampilkan papan yang sudah diperbarui setelah tembakan
                boardDisplay.RenderBoard(opponentBoard, true);
                char colLetter = (char)('A' + col); // Konversi angka ke huruf
                Console.WriteLine($"\n{currentPlayer.Name} menembak ke ({colLetter}{row + 1}) -> {result}");

                if (IsGameOver())
                {
                    Console.WriteLine($"\n{currentPlayer.Name} MENANG!");
                    OnGameOver?.Invoke(currentPlayer);
                    state = GameState.FINISHED;
                    return true;
                }
                Thread.Sleep(2000); //Revisi: pake task aja dan rapikan struktur folder: enum pisah, interface jadi 1 folder dll, hapus komen, ganti indo ke inggris kata-katanya
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

        //Memproses tembakan pada koordinat tertentu di papan lawan dan mengembalikan hasil tembakan (HIT, MISS, SUNK, dll.)
        public ShotResult ProcessShot(int row, int column)
        {
            if (state != GameState.PLAYING)
            {
                return ShotResult.INVALID;
            }
        
            IPlayer currentPlayer = playerOrder[currentPlayerIndex]; // Pemain yang sedang menembak
            IPlayer opponent = playerOrder[(currentPlayerIndex + 1) % playerOrder.Count]; // Lawan yang ditembak
            IBoard opponentBoard = boards[opponent]!; // Papan lawan

            if (!opponentBoard.IsPositionValid(row, column))
                return ShotResult.INVALID;

            CellStatus cellStatus = opponentBoard.GetCellStatus(row, column);

            if (cellStatus == CellStatus.HIT || cellStatus == CellStatus.MISS || cellStatus == CellStatus.SUNK)
                return ShotResult.ALREADY_SHOT;

            if (cellStatus == CellStatus.SHIP)
            {
                IShip? ship = opponentBoard.GetShipAt(row, column);
                if (ship != null && ship.RecordHit(row, column))
                {
                    opponentBoard.SetCellStatus(row, column, CellStatus.HIT);
                    if (ship.IsSunk())
                    {
                        foreach (var pos in ship.GetOccupiedPositions())
                            opponentBoard.SetCellStatus(pos.Item1, pos.Item2, CellStatus.SUNK);
                            OnShipSunk?.Invoke(ship);
                            if (IsGameOver())
                                state = GameState.FINISHED;
                        return ShotResult.SUNK;
                    }
                    return ShotResult.HIT;
                }
            }

            opponentBoard.SetCellStatus(row, column, CellStatus.MISS);
            return ShotResult.MISS;
        }

        //Mengganti giliran pemain dan memicu event OnTurnChanged untuk memberitahukan perubahan giliran
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

        //Mengembalikan pemain yang saat ini sedang memiliki giliran bermain
        public IPlayer GetCurrentPlayer()
        {
            return playerOrder[currentPlayerIndex];
        }

        //Mengembalikan papan pemain tertentu
        public IBoard GetPlayerBoard(IPlayer player)
        {
            return boards[player];
        }

        //Mengembalikan daftar kapal milik pemain
        public IReadOnlyList<IShip> GetPlayerShips(IPlayer player)
        {
            return playerShips[player].AsReadOnly();
        }
    }
}