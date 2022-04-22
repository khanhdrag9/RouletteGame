using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Asset;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class only for testing, it is not real server!
/// </summary>
public class ServerMockup
{
    public string ReceiveRequest(string method, string api, WWWForm form)
    {
        string response = "";
        if(method == UnityWebRequest.kHttpVerbGET)
        {
            if(api == "board/datas")
            {
                var jsonAssets = Resources.LoadAll<TextAsset>("ListBoardData");
                var boardDatas = new BoardData[jsonAssets.Length];

                for(int i = 0; i < jsonAssets.Length; i++)
                {
                    boardDatas[i] = JsonUtility.FromJson<BoardData>(jsonAssets[i].text);
                }

                response = JsonUtility.ToJson(new ListBoardData
                {
                    Value = boardDatas
                });
            }
        }
        else if(method == UnityWebRequest.kHttpVerbPOST)
        {

        }
        else if(method == UnityWebRequest.kHttpVerbPUT)
        {

        }

        return response;
    }


}
