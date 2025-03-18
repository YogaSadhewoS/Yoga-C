namespace BattleshipGame
{
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

        //Sudah ditangani di StartGame()
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

        public void StartGameLoop()
        {
            controller.InitializeGame(10, 2);
            controller.StartGame();
        }
    }
}