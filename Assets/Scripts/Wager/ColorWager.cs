namespace Game
{
    public class ColorWager : Wager
    {
        public override WagerType WagerType => WagerType.Color;

        public string ColorInString {get;}  // Equal to value/format is gotten from Game.Helper.Extensions.ColorToString()

        public ColorWager(int betAmount, float bonusRate, string colorStr) : base(betAmount, bonusRate)
        {
            this.ColorInString = colorStr;
        }

        public override bool IsRewardAble()
        {
            var result = ServiceLocator.GetService<BettingHistory>().GetLast();
            if(result != null)
            {
                return result.Color == ColorInString;
            }

            return false;
        }
    }
}