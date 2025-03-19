namespace BattleshipGame
{
    public class Player : IPlayer
    {
        private string name;
        private string id;
        private Dictionary<string, Dictionary<string, int>> statistics;

        public string Name => name;

        public Player(string name)
        {
            this.name = name;
            this.id = Guid.NewGuid().ToString();
            statistics = new Dictionary<string, Dictionary<string, int>>();
        }
        
        public void UpdateStatistic(string gameType, string stat, int value)
        {
            if (!statistics.ContainsKey(gameType))
                statistics[gameType] = new Dictionary<string, int>();
            if (!statistics[gameType].ContainsKey(stat))
                statistics[gameType][stat] = 0;
            statistics[gameType][stat] += value;
        }

        public int GetStatistic(string gameType, string stat)
        {
            if (statistics.ContainsKey(gameType) && statistics[gameType].ContainsKey(stat))
                return statistics[gameType][stat];
            return 0;
        }
    }
}