namespace Game.Bet
{
    /// <summary>
    /// All Bet class should implement this interface
    /// </summary>
    public interface IBet
    {
        int Currencies {get;}
        bool IsRewardAble();
        void Reward(Player player);
    }
}