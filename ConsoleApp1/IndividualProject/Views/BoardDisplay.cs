using BattleshipGame.Interfaces;
using BattleshipGame.Models;
using BattleshipGame.Enums;

namespace BattleshipGame.Views
{
    public class BoardDisplay : IBoardDisplay
    {
        public void RenderBoard(IBoard board, bool hideShips)
        {
            Console.Clear();
            int size = (board is Board b) ? b.Size : 10;
            Console.Write("    ");
            for (int j = 0; j < size; j++)
                Console.Write(((char)('A' + j)).ToString().PadLeft(2) + " ");
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                Console.Write((i + 1).ToString().PadLeft(2) + "  ");
                for (int j = 0; j < size; j++)
                {
                    CellStatus status = board.GetCellStatus(i, j);
                    string symbol = "|";
                    ConsoleColor color = Console.ForegroundColor;

                    if (status == CellStatus.HIT) { symbol = "O"; color = ConsoleColor.Blue; }
                    else if (status == CellStatus.MISS) { symbol = "X"; color = ConsoleColor.Red; }
                    else if (status == CellStatus.SUNK) { symbol = "S"; color = ConsoleColor.Green; }
                    else if (!hideShips && status == CellStatus.SHIP) { symbol = "|"; }

                    Console.ForegroundColor = color;
                    Console.Write(symbol.PadLeft(2) + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}
