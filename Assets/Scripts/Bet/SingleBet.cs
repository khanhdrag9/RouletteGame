namespace Game.Bet
{
    public class SingleBet : BetBase
    {
        int betNumber;

        public SingleBet(int betNumber, int betAmount) : base(betAmount)
        {
            this.betNumber = betNumber;
        }

        public override bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result == betNumber;
        }

        public override void Reward(Player player)
        {
            int reward = (int)(BetAmount * 1.9f);
            player.Bankroll.Receive(reward);
        }
    }
}