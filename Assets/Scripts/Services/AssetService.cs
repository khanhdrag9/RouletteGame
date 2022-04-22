using UnityEngine;
using Game.Asset;

namespace Game
{
    public class AssetService
    {
        private IAssetLoader assetLoader => new LocalResourcesLoader();
        public BoardData[] GetBoardData()
        {
            return assetLoader.GetBoardDatas();
        }

        public Sprite GetSprite(string defineName)
        {
            return assetLoader.GetAsset<Sprite>("Sprites/"+defineName);
        }
    }


#region Asset Loader implement
    interface IAssetLoader
    {
        BoardData[] GetBoardDatas();
        public T GetAsset<T>(string defineName) where T : Object;
    }

    class LocalResourcesLoader : IAssetLoader
    {
        const string StorageFolder = "ListBoardData";

        public BoardData[] GetBoardDatas()
        {
            var jsonAssets = Resources.LoadAll<TextAsset>(StorageFolder);
            var result = new BoardData[jsonAssets.Length];

            for(int i = 0; i < jsonAssets.Length; i++)
            {
                result[i] = JsonUtility.FromJson<BoardData>(jsonAssets[i].text);
            }

            return result;
        }

        public T GetAsset<T>(string defineName) where T : Object
        {
            var asset = Resources.Load<T>(defineName);
            return asset;
        }
    }

#endregion

}