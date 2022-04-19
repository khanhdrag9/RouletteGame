namespace Game.Bet
{
    public class SingleBetCreator : IBetCreator
    {
        private int betNumber;

        public SingleBetCreator(int betNumber)
        {
            this.betNumber = betNumber;
        }

        public BetBase GetBet(int betAmount)
        {
            return new SingleBet(betNumber, betAmount);
        }
    }
}