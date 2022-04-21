using Game.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform group;
        [SerializeField] private Button goGameModeBtnPrefab;

        private void Start()
        {
            var boardDatas = ServiceLocator.GetService<AssetService>().GetBoardData();
            for(int i = 0; i < boardDatas.Length; i++)
            {
                var data = boardDatas[i];
                var btn = Instantiate(goGameModeBtnPrefab, group);
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<Text>().text = data.Name;
                btn.onClick.AddListener(()=>
                {   
                    Global.BoardData = data;
                    SceneLoader.GoPlay();
                });
            }
        }
    }
}
