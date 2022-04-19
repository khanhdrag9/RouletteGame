using System.Collections;
using System.Collections.Generic;
using Game.Bet;
using UnityEngine;
using UnityEngine.UI;
using Game.Helper;

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
        [SerializeField] private ModeController modeController;
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
        public ModeController ModeController => modeController;
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

        private BoardData GetBoardData()
        {
            BoardData dataResult = null;

            // Can handle getting data from server

            // Test Data;
            dataResult = new BoardData
            {
                

                Numbers = new int[]
                {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19},

                AvailableBet = new BetData[]
                {
                    new BetData
                    {
                        Name = "single",
                        Visual = "Single Number",
                        BonusRate = 1.8f
                    },
                    new BetData
                    {
                        Name = "odd",
                        Visual = "Odd Number",
                        BonusRate = 1.4f
                    },
                    new BetData
                    {
                        Name = "even",
                        Visual = "Even Number",
                        BonusRate = 1.4f
                    }
                }
            };

            return dataResult;
        } 


#region Unity methods
        void Awake()
        {
            Instance = this;
            BoardState = BoardState.None;
        }

        void Start()
        {
            BoardData boardData = GetBoardData();
            modeController.Initalize(boardData);

            modeController.Player = player;
            BoardState = BoardState.Betting;
        }

        void Update()
        {
            playerCurrency.text = player.CurrencyCount.ToString();
        }
#endregion

    }
}
