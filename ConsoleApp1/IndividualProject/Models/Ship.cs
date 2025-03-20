using BattleshipGame.Interfaces;
using BattleshipGame.Models;
using BattleshipGame.Enums;

namespace BattleshipGame.Models
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

        public void PlaceAt(int row, int column, Orientation orientation)
        {
            this.startRow = row;
            this.startColumn = column;
            this.orientation = orientation;
        }

        public Position[] GetOccupiedPositions()
        {
            Position[] positions = new Position[Size];
            for (int i = 0; i < Size; i++)
            {
                int r = startRow + (orientation == Orientation.VERTICAL ? i : 0);
                int c = startColumn + (orientation == Orientation.HORIZONTAL ? i : 0);
                positions[i] = new Position(r, c);
            }
            return positions;
        }

        public bool IsPositionPartOfShip(int row, int column)
        {
            return GetOccupiedPositions().Any(pos => pos.Row == row && pos.Column == column);
        }

        public bool RecordHit(int row, int column)
        {
            var positions = GetOccupiedPositions();
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i].Row == row && positions[i].Column == column)
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