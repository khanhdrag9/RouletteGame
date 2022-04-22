using UnityEngine;
using Game.Asset;

namespace Game
{
    public class AssetService
    {
        private IAssetLoader assetLoader => new LocalResourcesLoader();
        public Sprite GetSprite(string defineName)
        {
            return assetLoader.GetAsset<Sprite>("Sprites/"+defineName);
        }
    }


#region Asset Loader implement
    interface IAssetLoader
    {
        public T GetAsset<T>(string defineName) where T : Object;
    }

    class LocalResourcesLoader : IAssetLoader
    {
        public T GetAsset<T>(string defineName) where T : Object
        {
            var asset = Resources.Load<T>(defineName);
            return asset;
        }
    }

#endregion

}