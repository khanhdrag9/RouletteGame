using UnityEngine;
using Game.Helper;
using Game.Asset;

namespace Game
{
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void Main()
        {
            ServiceLocator.Register<AssetService>(new AssetService());
        }
    }
}