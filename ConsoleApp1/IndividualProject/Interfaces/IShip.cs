using BattleshipGame.Models;

namespace BattleshipGame.Interfaces
{
    public interface IShip
    {
        bool RecordHit(int row, int column);
        bool IsSunk();
        bool IsPositionPartOfShip(int row, int column);
        Position[] GetOccupiedPositions();
    }
}