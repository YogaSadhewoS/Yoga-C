namespace BattleshipGame
{
    public class Ship : IShip
    {
        private ShipType type;
        private int startRow;
        private int startColumn;
        private Orientation orientation;
        private bool[] hitArray;

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
}