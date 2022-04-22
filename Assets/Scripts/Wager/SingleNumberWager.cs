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

        // Handle reward locally
        // public override bool IsRewardAble()
        // {
        //     var result = ServiceLocator.GetService<BettingHistory>().GetLast();
        //     if(result != null)
        //     {
        //         return result.Number == BetNumber;
        //     }

        //     return false;
        // }
    }
}