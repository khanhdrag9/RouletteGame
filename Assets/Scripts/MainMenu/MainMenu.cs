using Game.Helper;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game.Data;

namespace Game.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject loadingTxt;
        [SerializeField] private RectTransform group;
        [SerializeField] private Button goGameModeBtnPrefab;

        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            loadingTxt.SetActive(true);
            var boardDataRequest = new BoardDataRequest();
            yield return boardDataRequest;

            string jsonData = boardDataRequest.Response;
            if(string.IsNullOrEmpty(jsonData))
            {
                Debug.LogError("Couldn't get any board data/game mode from server");
                yield break;
            }

            var boardDatas = JsonUtility.FromJson<ListBoardData>(jsonData);
            for(int i = 0; i < boardDatas.Value.Length; i++)
            {
                var data = boardDatas.Value[i];
                var btn = Instantiate(goGameModeBtnPrefab, group);
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<Text>().text = data.Name;
                btn.onClick.AddListener(()=>
                {   
                    Global.BoardData = data;
                });
            }    
            loadingTxt.SetActive(false);
        }
    }
}
