namespace BattleshipGame
{
    public interface IBoard
    {
        bool PlaceShip(IShip ship, int row, int column, Orientation orientation);
        CellStatus GetCellStatus(int row, int column);
        void SetCellStatus(int row, int column, CellStatus status);

        // Mengembalikan kapal yang ditempatkan pada posisi (row, column), atau null jika tidak ada
        IShip? GetShipAt(int row, int column);
        bool IsPositionValid(int row, int column);
        IReadOnlyList<IShip> GetAllShips();
    }
}