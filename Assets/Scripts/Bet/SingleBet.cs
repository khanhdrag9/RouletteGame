namespace Game
{
    public class SingleBet : IBet
    {
        int betNumber;
        public int Currencies {get; private set;}

        public SingleBet(int betNumber, int currencies)
        {
            this.betNumber = betNumber;
            this.Currencies = currencies;
        }

        public bool IsRewardAble()
        {
            return PlayBoardManager.Instance.Result == betNumber;
        }

        public void Reward(Player player)
        {
            int reward = (int)(Currencies * 1.9f);
            player.Bankroll.Receive(reward);
        }

    }
}