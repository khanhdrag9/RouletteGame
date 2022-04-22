namespace Game
{
    public class RangeWager : Wager
    {
        public override WagerType WagerType => WagerType.Range;

        public int From {get;}
        public int To {get;}

        public RangeWager(int from, int to) : base()
        {
            this.From = from;
            this.To = to;
        }

        // Handle reward locally
        // public override bool IsRewardAble()
        // {
        //     var result = ServiceLocator.GetService<BettingHistory>().GetLast();
        //     if(result != null)
        //     {
        //         return From <= result.Number && result.Number <= To;
        //     }

        //     return false;
        // }
    }
}