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
    }


#region Asset Loader implement
    interface IAssetLoader
    {
        BoardData[] GetBoardDatas();
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
    }

#endregion

}