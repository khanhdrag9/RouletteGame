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
            var result = ServiceLocator.GetService<BettingHistory>().GetLast();
            if(result != null)
            {
                return From <= result.Number && result.Number <= To;
            }

            return false;
        }
    }
}