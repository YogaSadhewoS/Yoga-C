namespace BattleshipGame
{
    public class GameUI
    {
        private GameController controller;

        public GameUI(GameController controller)
        {
            this.controller = controller;
        }

        // Menampilkan status papan dan giliran pemain dengan menggunakan BoardDisplay.
        public void RenderGameState(Dictionary<IPlayer, IBoard> boards, IPlayer currentPlayer)
        {
            Console.WriteLine($"Giliran: {currentPlayer.Name}");
            IBoardDisplay boardDisplay = new BoardDisplay(); // gunakan BoardDisplay untuk merender papan

            foreach (var kvp in boards)
            {
                Console.WriteLine($"Papan {kvp.Key.Name}:");
                boardDisplay.RenderBoard(kvp.Value, true);
            }
        }

        // Menampilkan papan pemain tertentu dengan menggunakan BoardDisplay.
        public void RenderPlayerBoard(IPlayer player, IBoard board, bool hideShips)
        {
            Console.WriteLine($"Papan {player.Name}:");
            IBoardDisplay boardDisplay = new BoardDisplay();
            boardDisplay.RenderBoard(board, hideShips);
        }

        // Menampilkan pesan, yang sudah diatur oleh GameController.
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        // Jika nantinya ingin memasukkan kapal secara manual.
        public (ShipType, int, int, Orientation) GetPlacementInput()
        {
            Console.WriteLine("Masukkan jenis kapal (0: CARRIER, 1: BATTLESHIP, 2: CRUISER, 3: SUBMARINE, 4: DESTROYER): ");
            string? typeInputStr = Console.ReadLine();
            if (!int.TryParse(typeInputStr, out int typeInput))
            {
                Console.WriteLine("Input tidak valid untuk jenis kapal.");
                return GetPlacementInput();
            }
            // Gunakan operator ! karena kita yakin Enum.GetValues akan mengembalikan nilai
            ShipType type = (ShipType)Enum.GetValues(typeof(ShipType)).GetValue(typeInput)!;

            Console.WriteLine("Masukkan koordinat penempatan kapal (misal A5): ");
            string? input = Console.ReadLine()?.Trim().ToUpper();
            if (string.IsNullOrEmpty(input) || input.Length < 2)
            {
                Console.WriteLine("Input tidak valid untuk penempatan.");
                return GetPlacementInput();
            }
            char colChar = input[0];
            if (!char.IsLetter(colChar))
            {
                Console.WriteLine("Input tidak valid. Kolom harus berupa huruf.");
                return GetPlacementInput();
            }
            int col = colChar - 'A';
            if (!int.TryParse(input.Substring(1), out int row) || row < 1)
            {
                Console.WriteLine("Input tidak valid untuk baris.");
                return GetPlacementInput();
            }
            row -= 1; // Konversi ke indeks 0-based

            Console.WriteLine("Masukkan orientasi (0: HORIZONTAL, 1: VERTICAL): ");
            string? oriInputStr = Console.ReadLine();
            if (!int.TryParse(oriInputStr, out int oriInput))
            {
                Console.WriteLine("Input tidak valid untuk orientasi.");
                return GetPlacementInput();
            }
            Orientation orientation = (Orientation)oriInput;

            return (type, row, col, orientation);
        }

        // Mendapatkan input tembakan dengan validasi input koordinat.
        public (int, int) GetShotInput()
        {
            Console.WriteLine("Masukkan koordinat tembakan (misal A5): ");
            string? input = Console.ReadLine()?.Trim().ToUpper();
            if (string.IsNullOrEmpty(input) || input.Length < 2)
            {
                Console.WriteLine("Input tidak valid.");
                return GetShotInput();
            }
            char colChar = input[0];
            if (!char.IsLetter(colChar))
            {
                Console.WriteLine("Input tidak valid. Kolom harus huruf.");
                return GetShotInput();
            }
            int col = colChar - 'A';
            if (!int.TryParse(input.Substring(1), out int row) || row < 1)
            {
                Console.WriteLine("Input tidak valid untuk baris.");
                return GetShotInput();
            }
            row -= 1;
            return (row, col);
        }

        // Memulai loop permainan dengan menginisialisasi game dan memulai game.
        public void StartGameLoop()
        {
            controller.InitializeGame(10, 2);
            controller.StartGame();
        }
    }
}
