namespace Game.Bet
{
    /// <summary>
    /// Creator interface of Factory design pattern, creator of each kind of bet should implement this interface
    /// Use this pattern for easy customing many bet types
    /// </summary>
    public interface IBetCreator
    {
        /// <summary>
        /// Create a bet
        /// <param name="betAmount">bet amount of player</param>
        /// </summary>
        IBet GetBet(int betAmount);
    }
}