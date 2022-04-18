namespace Game.Player
{
    public interface IBet
    {
        void Initialize(int numberCurrencies);
        bool IsRewardAble();
        void Reward();
    }
}