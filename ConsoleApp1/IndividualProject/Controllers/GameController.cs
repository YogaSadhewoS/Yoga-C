using System;
using System.Collections.Generic;
using System.Threading;
using BattleshipGame.Interfaces;
using BattleshipGame.Models;
using BattleshipGame.Enums;

namespace BattleshipGame.Controllers
{
    public class GameController
    {
        private Dictionary<IPlayer, IBoard> boards;
        private Dictionary<IPlayer, List<IShip>> playerShips;
        private List<IPlayer> playerOrder;
        private int currentPlayerIndex;
        private GameState state;

        public event Action<IPlayer>? OnTurnChanged;
        public event Action<IPlayer>? OnGameOver;
        public event Action<IShip>? OnShipSunk;
        public event Action<int, int, ShotResult>? OnShotProcessed;

        private int boardSize;

        public GameController()
        {
            boards = new Dictionary<IPlayer, IBoard>();
            playerShips = new Dictionary<IPlayer, List<IShip>>();
            playerOrder = new List<IPlayer>();
            currentPlayerIndex = 0;
            state = GameState.SETUP;
        }

        public void InitializeGame(int boardSize, List<string> playerNames)
        {
            this.boardSize = boardSize;
            for (int i = 0; i < playerNames.Count; i++)
            {
                string name = playerNames[i];
                IPlayer player = new Player(name);
                playerOrder.Add(player);
                boards[player] = new Board(boardSize);
                playerShips[player] = new List<IShip>();

                ShipFactory factory = new ShipFactory();
                List<IShip> ships = factory.CreateStandardShipSet();
                Random rand = new Random();
                foreach (IShip ship in ships)
                {
                    bool placed = false;
                    while (!placed)
                    {
                        int row = rand.Next(boardSize);
                        int col = rand.Next(boardSize);
                        Orientation orientation = (rand.Next(2) == 0) ? Orientation.HORIZONTAL : Orientation.VERTICAL;
                        if (boards[player].PlaceShip(ship, row, col, orientation))
                        {
                            placed = true;
                            playerShips[player].Add(ship);
                        }
                    }
                }
            }
            state = GameState.PLAYING;
        }

        public ShotResult ProcessShot(int row, int column)
        {
            if (state != GameState.PLAYING)
                return ShotResult.INVALID;

            IPlayer currentPlayer = playerOrder[currentPlayerIndex];
            IPlayer opponent = GetOpponent();
            IBoard opponentBoard = boards[opponent]!;

            if (!opponentBoard.IsPositionValid(row, column))
                return ShotResult.INVALID;

            CellStatus cellStatus = opponentBoard.GetCellStatus(row, column);

            if (cellStatus == CellStatus.HIT || cellStatus == CellStatus.MISS || cellStatus == CellStatus.SUNK)
                return ShotResult.ALREADY_SHOT;

            if (cellStatus == CellStatus.SHIP)
            {
                IShip? ship = opponentBoard.GetShipAt(row, column);
                if (ship != null && ship.RecordHit(row, column))
                {
                    opponentBoard.SetCellStatus(row, column, CellStatus.HIT);
                    if (ship.IsSunk())
                    {
                        foreach (var pos in ship.GetOccupiedPositions())
                            opponentBoard.SetCellStatus(pos.Row, pos.Column, CellStatus.SUNK);
                        OnShipSunk?.Invoke(ship);
                        if (IsGameOver())
                            state = GameState.FINISHED;
                        return ShotResult.SUNK;
                    }
                    return ShotResult.HIT;
                }
            }

            opponentBoard.SetCellStatus(row, column, CellStatus.MISS);
            return ShotResult.MISS;
        }

        public void SwitchTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playerOrder.Count;
            OnTurnChanged?.Invoke(GetCurrentPlayer());
        }

        public bool IsGameOver()
        {
            IPlayer opponent = GetOpponent();
            foreach (IShip ship in boards[opponent].GetAllShips())
            {
                if (!ship.IsSunk())
                    return false;
            }
            return true;
        }

        public IPlayer GetCurrentPlayer()
        {
            return playerOrder[currentPlayerIndex];
        }

        public IPlayer GetOpponent()
        {
            return playerOrder[(currentPlayerIndex + 1) % playerOrder.Count];
        }

        public IBoard GetPlayerBoard(IPlayer player)
        {
            return boards[player];
        }

        public IReadOnlyList<IShip> GetPlayerShips(IPlayer player)
        {
            return playerShips[player].AsReadOnly();
        }
    }
}
