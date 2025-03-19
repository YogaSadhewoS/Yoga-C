namespace BattleshipGame
{
    // Interface untuk menangani tampilan papan permainan.
    public interface IBoardDisplay
    {
        void RenderBoard(IBoard board, bool hideShips);
    }
}
