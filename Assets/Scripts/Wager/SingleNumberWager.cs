namespace Game
{
    public class SingleNumberWager : Wager
    {
        public override WagerType WagerType => WagerType.Single;

        public int BetNumber {get;}

        public SingleNumberWager(int betAmount, float bonusRate, int betNumber) : base(betAmount, bonusRate)
        {
            this.BetNumber = betNumber;
        }

        public override bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result == BetNumber;
        }
    }
}