using BattleshipGame.Interface;

namespace BattleshipGame
{
    public class Board : IBoard
    {
        private int size;
        private CellStatus[,] cellStatus;

        // Array dua dimensi yang menyimpan referensi kapal di tiap sel, null jika sel kosong
        private IShip?[,] cellShips; 
        private List<IShip> ships;

        public int Size => size;

        //Inisialisasi papan dengan ukuran tertentu
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

        //Cek apakah posisi row column ada di board
        public bool IsPositionValid(int row, int column)
        {
            return row >= 0 && row < size && column >= 0 && column < size;
        }

        // Menempatkan kapal pada papan di posisi dan orientasi tertentu jika semua posisi valid dan kosong
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

         // Mengembalikan status sel pada posisi (row, column), throw exception jika posisi tidak valid
        public CellStatus GetCellStatus(int row, int column)
        {
            if (!IsPositionValid(row, column))
                throw new ArgumentException("Invalid position"); //Revisi: Jangan pake throw
            return cellStatus[row, column];
        }

        // Mengatur status sel pada posisi tertentu jika posisi valid
        public void SetCellStatus(int row, int column, CellStatus status)
        {
            if (IsPositionValid(row, column))
                cellStatus[row, column] = status;
        }

        // Mengembalikan kapal yang ditempatkan di posisi tertentu, atau null jika tidak ada
        public IShip? GetShipAt(int row, int column)
        {
            if (IsPositionValid(row, column))
                return cellShips[row, column];
            return null;
        }

        // Mengembalikan daftar read-only dari semua kapal yang ada di papan
        public IReadOnlyList<IShip> GetAllShips()
        {
            return ships.AsReadOnly();
        }
    }
}