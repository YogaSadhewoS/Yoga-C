using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BattleshipGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameController game = new GameController();
            GameUI ui = new GameUI(game);
            ui.StartGameLoop();

            Console.WriteLine("Tekan sembarang tombol untuk keluar...");
            Console.ReadKey();
        }
    }
}
