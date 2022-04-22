namespace Game
{
    public class ColorWager : Wager
    {
        public override WagerType WagerType => WagerType.Color;

        public string ColorInString {get;}  // Equal to value/format is gotten from Game.Helper.Extensions.ColorToString()

        public ColorWager(string colorStr) : base()
        {
            this.ColorInString = colorStr;
        }

        // Handle reward locally
        // public override bool IsRewardAble()
        // {
        //     var result = ServiceLocator.GetService<BettingHistory>().GetLast();
        //     if(result != null)
        //     {
        //         return result.Color == ColorInString;
        //     }

        //     return false;
        // }
    }
}