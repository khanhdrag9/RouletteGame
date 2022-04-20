using System.Collections;
using System.Collections.Generic;
using Game.Bet;
using UnityEngine;
using UnityEngine.UI;
using Game.Helper;
using Game.Asset;

namespace Game
{
    public enum BoardState
    {
        None,
        Betting,
        Spinning
    }

    public class PlayBoardManager : MonoBehaviour
    {
        [System.Serializable]
        public class BoardStateData
        {
            public BoardState State;
            public BoardStateBase Component;
        }

        public static PlayBoardManager Instance {get; private set;}


        [Header("General")]
        [SerializeField] private Player player;
        [SerializeField] private PlayBoardController playBoardController;
        [SerializeField] private List<BoardStateData> stateDatas;

        [Header("GUI")]
        [SerializeField] private Text playerCurrency;

        [Header("Debug")]
        [SerializeField] private int debugSpinResult = 0;

        private BoardState boardState = BoardState.None;

        // public int Result {get; private set;} = -1;
        public int Result => debugSpinResult;
        public Player Player => player;
        public BoardState BoardState
        {
            get => boardState;
            set
            {
                if(value == BoardState.None)
                {
                    foreach(var state in stateDatas)
                        state.Component.enabled = false;
                }
                else
                {
                    if(boardState == value) return;

                    foreach(var state in stateDatas)
                    {
                        // Current state
                        if(state.State == boardState)   
                            state.Component.enabled = false;
                        
                        // New State
                        if(state.State == value)    
                            state.Component.enabled = true;
                    }
                }

                boardState = value;
            }
        }

#region Unity methods
        void Awake()
        {
            Instance = this;
            BoardState = BoardState.None;
        }

        void Start()
        {
            BoardData[] boardDatas = ServiceLocator.GetService<AssetService>().GetBoardData();
            playBoardController.Initialize(boardDatas[0]);

            BoardState = BoardState.Betting;
        }

        void Update()
        {
            playerCurrency.text = player.CurrencyCount.ToString();
        }
#endregion

    }
}
