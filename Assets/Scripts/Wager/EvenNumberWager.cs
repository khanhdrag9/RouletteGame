namespace Game
{
    public class EvenNumberWager : Wager
    {
        public override WagerType WagerType => WagerType.Even;

        public EvenNumberWager(int betAmount, float bonusRate) : base(betAmount, bonusRate)
        {
        }

        public override bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result % 2 == 0;
        }
    }
}