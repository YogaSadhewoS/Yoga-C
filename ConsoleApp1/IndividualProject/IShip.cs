namespace BattleshipGame
{
    // === INTERFACE ISHIP DAN IMPLEMENTASI SHIP ===
    public interface IShip
    {
        bool RecordHit(int row, int column);
        bool IsSunk();
        bool IsPositionPartOfShip(int row, int column);
        (int, int)[] GetOccupiedPositions();
    }
}