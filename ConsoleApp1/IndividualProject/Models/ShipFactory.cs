using BattleshipGame.Interfaces;
using BattleshipGame.Enums;

namespace BattleshipGame.Models
{
    public class ShipFactory
    {
        public IShip CreateShip(ShipType type)
        {
            return new Ship(type);
        }

        public List<IShip> CreateStandardShipSet()
        {
            return new List<IShip>
            {
                CreateShip(ShipType.CARRIER),
                CreateShip(ShipType.BATTLESHIP),
                CreateShip(ShipType.CRUISER),
                CreateShip(ShipType.SUBMARINE),
                CreateShip(ShipType.DESTROYER)
            };
        }
    }
}