namespace Game.Bet
{
    /// <summary>
    /// All Bet class should implement this abstract
    /// </summary>
    public abstract class BetBase
    {
        /// <summary>
        /// an unique id
        /// </summary>
        public string Id {get;}

        /// <summary>
        /// bet amount that player placed
        /// </summary>
        public int BetAmount {get; set;}

        protected BetBase(int currency)
        {
            Id = System.Guid.NewGuid().ToString("N");
            BetAmount = currency;
        }
        
        public abstract bool IsRewardAble();
        public abstract void Reward(Player player);
    }
}