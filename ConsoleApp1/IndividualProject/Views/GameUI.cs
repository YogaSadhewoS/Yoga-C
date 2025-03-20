using System;
using System.Collections.Generic;
using System.Threading;
using BattleshipGame.Interfaces;
using BattleshipGame.Controllers;
using BattleshipGame.Models;
using BattleshipGame.Enums;

namespace BattleshipGame.Views
{
    public class GameUI
    {
        private GameController controller;
        private IBoardDisplay boardDisplay;

        public GameUI(GameController controller)
        {
            this.controller = controller;
            boardDisplay = new BoardDisplay();
        }

        public void RenderGameState(Dictionary<IPlayer, IBoard> boards, IPlayer currentPlayer)
        {
            Console.WriteLine($"Turn: {currentPlayer.Name}");
            foreach (var kvp in boards)
            {
                Console.WriteLine($"Board for {kvp.Key.Name}:");
                boardDisplay.RenderBoard(kvp.Value, true);
            }
        }

        public void RenderPlayerBoard(IPlayer player, IBoard board, bool hideShips)
        {
            Console.WriteLine($"Board for {player.Name}:");
            boardDisplay.RenderBoard(board, hideShips);
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public (ShipType, int, int, Orientation) GetPlacementInput()
        {
            Console.WriteLine("Enter ship type (0: CARRIER, 1: BATTLESHIP, 2: CRUISER, 3: SUBMARINE, 4: DESTROYER): ");
            string? typeInputStr = Console.ReadLine();
            if (!int.TryParse(typeInputStr, out int typeInput))
            {
                Console.WriteLine("Invalid ship type input.");
                return GetPlacementInput();
            }
            ShipType type = (ShipType)Enum.GetValues(typeof(ShipType)).GetValue(typeInput)!;

            Console.WriteLine("Enter ship placement coordinates (e.g., A5): ");
            string? input = Console.ReadLine()?.Trim().ToUpper();
            if (string.IsNullOrEmpty(input) || input.Length < 2)
            {
                Console.WriteLine("Invalid placement input.");
                return GetPlacementInput();
            }
            char colChar = input[0];
            if (!char.IsLetter(colChar))
            {
                Console.WriteLine("Invalid column. Must be a letter.");
                return GetPlacementInput();
            }
            int col = colChar - 'A';
            if (!int.TryParse(input.Substring(1), out int row) || row < 1)
            {
                Console.WriteLine("Invalid row input.");
                return GetPlacementInput();
            }
            row -= 1;

            Console.WriteLine("Enter orientation (0: HORIZONTAL, 1: VERTICAL): ");
            string? oriInputStr = Console.ReadLine();
            if (!int.TryParse(oriInputStr, out int oriInput))
            {
                Console.WriteLine("Invalid orientation input.");
                return GetPlacementInput();
            }
            Orientation orientation = (Orientation)oriInput;

            return (type, row, col, orientation);
        }

        public (int, int) GetShotInput()
        {
            bool valid = false;
            bool printedError = false;
            (int row, int col) shot = (0, 0);

            while (!valid)
            {
                if (!printedError)
                    Console.WriteLine("Enter shot coordinates (e.g., A5 or a5): ");
                else
                    Console.Write("Enter shot coordinates (e.g., A5 or a5): ");

                string? input = Console.ReadLine()?.Trim().ToUpper();
                if (string.IsNullOrEmpty(input) || input.Length < 2)
                {
                    if (!printedError)
                    {
                        Console.WriteLine("Invalid shot input.");
                        printedError = true;
                    }
                    continue;
                }
                char colChar = input[0];
                if (!char.IsLetter(colChar))
                {
                    if (!printedError)
                    {
                        Console.WriteLine("Invalid column. Must be a letter.");
                        printedError = true;
                    }
                    continue;
                }
                int col = colChar - 'A';
                if (!int.TryParse(input.Substring(1), out int row) || row < 1)
                {
                    if (!printedError)
                    {
                        Console.WriteLine("Invalid row input.");
                        printedError = true;
                    }
                    continue;
                }
                shot = (row - 1, col);
                valid = true;
            }
            return shot;
        }

        public async Task StartGameLoop()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"Enter name for Player {i + 1} (leave blank for default):");
                string? name = Console.ReadLine();
                names.Add(string.IsNullOrWhiteSpace(name) ? $"Player {i + 1}" : name);
            }
            controller.InitializeGame(10, names);


            while (!controller.IsGameOver())
            {
                IPlayer currentPlayer = controller.GetCurrentPlayer();
                IPlayer opponent = controller.GetOpponent();
                IBoard opponentBoard = controller.GetPlayerBoard(opponent);

                boardDisplay.RenderBoard(opponentBoard, true);
                Console.WriteLine($"\n{currentPlayer.Name} turn. Enter shot coordinates (e.g., A5): ");
                (int row, int col) = GetShotInput();

                ShotResult result = controller.ProcessShot(row, col);

                if (result == ShotResult.ALREADY_SHOT)
                {
                    Console.WriteLine("Already shot at this location. Try a different coordinate.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                boardDisplay.RenderBoard(opponentBoard, true);
                char colLetter = (char)('A' + col);
                Console.WriteLine($"\n{currentPlayer.Name} shot at ({colLetter}{row + 1}) -> {result}");

                if (controller.IsGameOver())
                {
                    Console.WriteLine($"\n{currentPlayer.Name} wins!");
                    break;
                }
                await Task.Delay(2000);
                controller.SwitchTurn();
            }
        }
    }
}
