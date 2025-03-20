using BattleshipGame.Interfaces;

namespace BattleshipGame
{
    public interface IBoardDisplay
    {
        void RenderBoard(IBoard board, bool hideShips);
    }
}
