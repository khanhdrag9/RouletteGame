namespace Game.Bet
{
    public class EvenNumberBet : BetBase
    {
        public EvenNumberBet(int betAmount, float bonusRate) : base(betAmount, bonusRate)
        {
        }

        public override bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result % 2 == 0;
        }
    }
}