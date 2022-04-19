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
        
        /// <summary>
        /// BonusRate x BetAmount = reward of player
        /// </summary>
        public float BonusRate {get; set;}

        protected int reward;

        protected BetBase(int betAmount, float bonusRate)
        {
            Id = System.Guid.NewGuid().ToString("N");
            this.BetAmount = betAmount;
            this.BonusRate = bonusRate;
        }
        
        public abstract bool IsRewardAble();
        public virtual void Reward(Player player)
        {
            reward = (int)(BetAmount * BonusRate);
            player.Reward(reward);
        }
    }
}