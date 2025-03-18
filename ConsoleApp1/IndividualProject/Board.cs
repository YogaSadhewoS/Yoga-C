namespace BattleshipGame
{
    public class Board : IBoard
    {
        private int size;
        private CellStatus[,] cellStatus;
        private IShip?[,] cellShips;
        private List<IShip> ships;

        public int Size => size;

        public Board(int size)
        {
            this.size = size;
            cellStatus = new CellStatus[size, size];
            cellShips = new IShip?[size, size];
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

        public IShip? GetShipAt(int row, int column)
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
                Console.Write(((char)('A' + j)).ToString().PadLeft(2) + " ");
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                Console.Write((i + 1).ToString().PadLeft(2) + "  ");
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
}