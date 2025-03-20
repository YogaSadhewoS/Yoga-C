using BattleshipGame.Interfaces;

namespace BattleshipGame.Models
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
            Ship s = ship as Ship;
            if (s == null)
                return false;

            s.PlaceAt(row, column, orientation);
            var positions = s.GetOccupiedPositions();

            foreach (var pos in s.GetOccupiedPositions())
            {
                if (!IsPositionValid(pos.Row, pos.Column) || cellStatus[pos.Row, pos.Column] != CellStatus.EMPTY)
                    return false;
            }

            foreach (var pos in s.GetOccupiedPositions())
            {
                cellStatus[pos.Row, pos.Column] = CellStatus.SHIP;
                cellShips[pos.Row, pos.Column] = ship;
            }
            ships.Add(ship);
            return true;
        }

        public CellStatus GetCellStatus(int row, int column)
        {
            if (!IsPositionValid(row, column))
                throw new ArgumentException("Invalid position"); //Revisi: Jangan pake throw
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
    }
}