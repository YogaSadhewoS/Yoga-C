namespace BattleshipGame.Interface
{
    public interface IPlayer
    {
        string Name { get; }
        void UpdateStatistic(string gameType, string stat, int value);
        int GetStatistic(string gameType, string stat);
    }
}