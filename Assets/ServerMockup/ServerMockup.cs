using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Asset;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class only for testing/demo, it is not real server!
/// </summary>
public class ServerMockup
{
    static BoardData[] boardDatas;
    public string ReceiveRequest(string method, string api, string data)
    {
        string response = "";
        if(method == UnityWebRequest.kHttpVerbGET)
        {
            // Request get board datas / game modes
            if(api == "board/datas")
            {
                var jsonAssets = Resources.LoadAll<TextAsset>("ListBoardData");
                boardDatas = new BoardData[jsonAssets.Length];

                for(int i = 0; i < jsonAssets.Length; i++)
                {
                    boardDatas[i] = JsonUtility.FromJson<BoardData>(jsonAssets[i].text);
                }

                response = JsonUtility.ToJson(new ListBoardData
                {
                    Value = boardDatas
                });
            }

            // Request place wager and get result
            if(api == "wheel/result")
            {   
                var singleWagers = Global.BoardData.Boxes.Where(b => b.Name == WagerType.Single.ToString()).ToArray();
                string strParam = "";

                // This is result that is created randomly
                int randomIndex = Random.Range(0, singleWagers.Length);
                strParam = singleWagers[randomIndex].StrParam;
                // If want to control result, set strParam to number you expect
                // strParam = "10";    // result is 10, 10 is NUMBER that player bet in, even the icons also are assigned to 1 number  

                var responseObject = new WagerResponse
                {
                    Result = int.Parse(strParam),    // Because it is Single Wager so it always be number
                    RewardAmount = 0
                };
                var requestObj = JsonUtility.FromJson<WagerRequestData>(data);
                var board = boardDatas.First(e => e.Name == requestObj.GameMode);
                foreach(var wager in requestObj.Wagers)
                {
                    bool isReward = false;
                    if (wager.WagerType == WagerType.Range.ToString())
                    {
                        int intResult = int.Parse(strParam);
                        var p = wager.StrParam.Split('-');
                        int from = int.Parse(p[0]);
                        int to = int.Parse(p[1]);
                        isReward = from <= intResult && intResult <= to;
                    }
                    else if (wager.WagerType == WagerType.Odd.ToString())
                    {
                        int intResult = int.Parse(strParam);
                        isReward = intResult % 2 == 1;
                    }
                    else if (wager.WagerType == WagerType.Even.ToString())
                    {
                        int intResult = int.Parse(strParam);
                        isReward = intResult % 2 == 0;
                    }
                    else if (wager.WagerType == WagerType.Color.ToString())
                    {
                        string colorOfResult = singleWagers.First(e => e.StrParam == strParam).Color;
                        isReward = colorOfResult == wager.StrParam;
                    }
                    // else if(wager.WagerType == WagerType.Single.ToString())
                    else
                    {
                        try
                        {
                            isReward = int.Parse(wager.StrParam) == int.Parse(strParam);
                        }
                        catch
                        {
                            isReward = false;
                        }
                    }

                    if(isReward)
                    {
                        var designOfWager = board.Boxes.First(e => e.Name == wager.WagerType && e.StrParam == wager.StrParam);
                        float bonusRate = designOfWager.FloatParam;
                        responseObject.RewardAmount += (int)(wager.BetAmount * bonusRate);
                    }
                }
                
                response = JsonUtility.ToJson(responseObject);
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
