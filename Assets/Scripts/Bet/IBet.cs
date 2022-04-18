namespace Game
{
    public interface IBet
    {
        int Currencies {get;}
        bool IsRewardAble();
        void Reward(Player player);
    }
}