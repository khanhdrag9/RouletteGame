namespace Game
{
    public class OddNumberWager : Wager
    {
        public override WagerType WagerType => WagerType.Odd;

        public OddNumberWager(int betAmount, float bonusRate) : base(betAmount, bonusRate)
        {
        }

        public override bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result % 2 == 1;
        }
    }
}