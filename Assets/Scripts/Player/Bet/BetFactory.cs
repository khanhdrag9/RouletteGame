namespace Game.Player
{
    public enum BetType
    {
        SingleBet
    }

    public class BetFactory
    {
        public IBet GetBet(BetType type)
        {
            switch (type)  
            {  
                case BetType.SingleBet:
                    return new SingleBet();  
                default:  
                    return new SingleBet();  
            }   
        }
    }
}