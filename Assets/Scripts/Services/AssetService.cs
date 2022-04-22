using UnityEngine;

namespace Game
{
    /// <summary>
    /// This class is used to get/load assets in game
    /// </summary>
    public class AssetService
    {
        private IAssetLoader assetLoader => new LocalResourcesLoader();
        public Sprite GetSprite(string defineName)
        {
            return assetLoader.GetAsset<Sprite>("Sprites/"+defineName);
        }
    }


#region Asset Loader implement

    /// <summary>
    /// How about integrating new loading types: Assetbundle, Addressable, from StreammingAssets,...
    /// Implement this interface for intergrating new loading type
    /// </summary>  
    interface IAssetLoader
    {
        T GetAsset<T>(string defineName) where T : Object;
    }

    /// <summary>
    /// Load/Get asset from Resources
    /// </summary>   
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