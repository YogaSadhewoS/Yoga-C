using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BattleshipGame.Controllers;
using BattleshipGame.Views;

namespace BattleshipGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameController game = new GameController();
            GameUI ui = new GameUI(game);
            ui.StartGameLoop().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
