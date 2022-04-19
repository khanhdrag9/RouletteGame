namespace Game.Bet
{
    public class SingleBetCreator : IBetCreator
    {
        private int betNumber;

        public SingleBetCreator(int betNumber)
        {
            this.betNumber = betNumber;
        }

        public IBet GetBet(int betAmount)
        {
            return new SingleBet(betNumber, betAmount);
        }
    }
}