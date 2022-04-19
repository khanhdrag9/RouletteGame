namespace Game.Bet
{
    public class SingleNumberBet : BetBase
    {
        int betNumber;

        public SingleNumberBet(int betAmount, float bonusRate, int betNumber) : base(betAmount, bonusRate)
        {
            this.betNumber = betNumber;
        }

        public override bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result == betNumber;
        }
    }
}