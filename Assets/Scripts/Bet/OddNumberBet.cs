namespace Game.Bet
{
    public class OddNumberBet : BetBase
    {
        public OddNumberBet(int betAmount, float bonusRate) : base(betAmount, bonusRate)
        {
        }

        public override bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result % 2 == 1;
        }
    }
}