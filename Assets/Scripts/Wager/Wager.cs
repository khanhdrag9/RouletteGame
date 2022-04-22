namespace Game
{
    /// <summary>
    /// All Bet class should implement this abstract
    /// </summary>
    public abstract class Wager
    {
        /// <summary>
        /// an unique id
        /// </summary>
        public string Id {get;}

        /// <summary>
        /// bet amount that player placed
        /// </summary>
        public int BetAmount
        {
            get => betAmnout;
            set => betAmnout = value >= 0 ? value : 0;
        }
        
        /// <summary>
        /// BonusRate x BetAmount = reward of player
        /// </summary>
        public float BonusRate {get; set;}

        /// <summary>
        /// Equal StrParam from data design GUIObjectData.StrParam
        /// </summary>
        public string RawStrParam {get; set;}

        public abstract WagerType WagerType {get;}

        protected int reward;

        private int betAmnout; 

        protected Wager()
        {
            Id = System.Guid.NewGuid().ToString("N");
        }
        
        // Handle reward locally
        // public abstract bool IsRewardAble();
        public virtual void Reward(Player player)
        {
            reward = (int)(BetAmount * BonusRate);
            player.Reward(reward);
        }
    }
}