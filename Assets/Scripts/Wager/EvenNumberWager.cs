namespace Game
{
    public class EvenNumberWager : Wager
    {
        public override WagerType WagerType => WagerType.Even;

        public EvenNumberWager() : base()
        {
        }

        // Handle reward locally
        // public override bool IsRewardAble()
        // {
        //     var result = ServiceLocator.GetService<BettingHistory>().GetLast();
        //     if(result != null)
        //     {
        //         return result.Number % 2 == 0;
        //     }

        //     return false;
        // }
    }
}