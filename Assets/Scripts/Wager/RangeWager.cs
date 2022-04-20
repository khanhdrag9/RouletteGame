namespace Game
{
    public class RangeWager : Wager
    {
        public override WagerType WagerType => WagerType.Range;

        public int From {get;}
        public int To {get;}

        public RangeWager(int betAmount, float bonusRate, int from, int to) : base(betAmount, bonusRate)
        {
            this.From = from;
            this.To = to;
        }

        public override bool IsRewardAble()
        {
            int result = PlayBoardManager.Instance.Result;
            return From <= result && result <= To;
        }
    }
}