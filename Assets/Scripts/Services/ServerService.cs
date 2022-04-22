using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
#region For Coroutine
    public class BoardDataRequest : CustomYieldInstruction
    {
        public string Response {get; private set;}
        public override bool keepWaiting => Time.time < time || inProgress;
        private bool inProgress = true;
        private float time;
        
        public BoardDataRequest()
        {
            inProgress =  true;
            // Request server to get here...

            // I use ServerMockup for demo purpose
            time = Time.time + 3f;  // Delay 3f
            Response = new ServerMockup().ReceiveRequest(UnityWebRequest.kHttpVerbGET, "board/datas", null);

            inProgress = false;
        }
    }

    public class WagerRequest : CustomYieldInstruction
    {
        public string Response {get; private set;}
        public override bool keepWaiting => Time.unscaledTime < time || inProgress;
        private bool inProgress = true;
        private float time;

        public WagerRequest(Wager[] wagers)
        {
            inProgress = true;

            // Handle request to server here....
            var requestData = new WagerRequestData
            {
                GameMode = Global.BoardData.Name,
                Wagers = new WagerData[wagers.Length]
            };
            
            for(int i = 0; i < wagers.Length; i++)
            {
                requestData.Wagers[i] = new WagerData
                {
                    WagerType = wagers[i].WagerType.ToString(),
                    StrParam = wagers[i].RawStrParam,
                    BetAmount = wagers[i].BetAmount
                };
            }

            var requestJson = JsonUtility.ToJson(requestData);
            // Request server to get here...

            // I use ServerMockup for demo purpose
            time = Time.unscaledTime + 1f;  // Delay 1f
            Response = new ServerMockup().ReceiveRequest(UnityWebRequest.kHttpVerbGET, "wheel/result", requestJson);

            inProgress = false;
        }
    }

#endregion

    [System.Serializable]
    public class WagerRequestData
    {
        public string GameMode;
        public WagerData[] Wagers;
    }

    [System.Serializable]
    public class WagerData
    {
        public string WagerType;
        public string StrParam;
        public int BetAmount;
    }

    [System.Serializable]
    public class WagerResponse
    {
        public int Result;
        public int RewardAmount;
    }
}