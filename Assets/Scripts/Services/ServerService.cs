using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

namespace Game
{
    public class ServerService
    {
        public WagerRequest SendWagersToServer(Wager[] wagers)
        {
            return new WagerRequest(wagers);
        }

        public class WagerRequest : CustomYieldInstruction
        {
            public WagerResponse Response {get; private set;}
            public override bool keepWaiting => inProgress;

            private bool inProgress = false;

            public WagerRequest(Wager[] wagers)
            {
                inProgress = true;

                // Handle request to server here....
                var requestData = new WagerRequestData
                {
                    GameMode = Global.BoardData.Name,
                    wagers = new WagerData[wagers.Length]
                };
                
                for(int i = 0; i < wagers.Length; i++)
                {
                    requestData.wagers[i] = new WagerData
                    {
                        WagerType = wagers[i].WagerType.ToString(),
                        BetAmount = wagers[i].BetAmount
                    };
                }

                string json = JsonUtility.ToJson(requestData);
                // Send here....
                // ....
                // End


                // TEST: create a result randomly here
                var singleWagers = Global.BoardData.Boxes.Where(b => b.Name == WagerType.Single.ToString()).ToArray();
                int randomIndex = Random.Range(0, singleWagers.ToArray().Length);
                var resultItem = singleWagers[randomIndex];

                Response = new WagerResponse
                {
                    Result = $"{resultItem.StrParam};{resultItem.Color}"
                };

                inProgress = false;
            }
        }


        [System.Serializable]
        public class WagerRequestData
        {
            public string GameMode;
            public WagerData[] wagers;
        }

        [System.Serializable]
        public class WagerData
        {
            public string WagerType;
            public int BetAmount;
        }

        [System.Serializable]
        public class WagerResponse
        {
            public string Result;
        }
    }
}