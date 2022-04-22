namespace Game
{
    public class OddNumberWager : Wager
    {
        public override WagerType WagerType => WagerType.Odd;

        public OddNumberWager(int betAmount, float bonusRate) : base(betAmount, bonusRate)
        {
        }

        // Handle reward locally
        // public override bool IsRewardAble()
        // {
        //     var result = ServiceLocator.GetService<BettingHistory>().GetLast();
        //     if(result != null)
        //     {
        //         return result.Number % 2 == 1;
        //     }

        //     return false;
        // }
    }
}