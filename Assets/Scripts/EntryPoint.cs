using UnityEngine;

namespace Game
{
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void Main()
        {
            ServiceLocator.Register<AssetService>(new AssetService());
            ServiceLocator.Register<BettingHistory>(new BettingHistory());
            ServiceLocator.Register<ServerService>(new ServerService());
        }
    }
}