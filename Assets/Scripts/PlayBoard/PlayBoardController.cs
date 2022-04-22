using System.Collections;
using System.Collections.Generic;
using Game.Asset;
using Game.Helper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class PlayBoardController : MonoBehaviour
    {
        [Header("Play board")]
        [SerializeField] private GameObject wagerBoxPrefab;
        [SerializeField] private RectTransform wagerBoxParent;
        [SerializeField] private Button spinBtn;
        [SerializeField] private GameObject circleSpinnerPrefab;
        [SerializeField] private RectTransform spinnerParent;

        [Header("About currency")]
        [SerializeField] private InputField playerCurrencyTxt;
        [SerializeField] private InputField betAmountInput;
        [SerializeField] private InputField totalBetAmountTxt;
        [SerializeField] private int startBetAmount = 10;

        [Header("Result")]
        [SerializeField] private GameObject resultLayout;
        [SerializeField] private Text resultBetAmountTxt;
        [SerializeField] private Text resultRewardTxt;
        [SerializeField] private Button closeResultBtn;

 
        private PlayBoardManager playBoardManager;
        private Player player => playBoardManager.Player;
        private List<NumberOnBetBoardGUI> wagerBoxGUIs;
        private List<Wager> wagers;
        private ISpinner spinner;
        private IState state;
        private bool blockBetting;
        private int betAmount
        {
            get 
            {
                if(int.TryParse(betAmountInput.text, out int result))
                    return result;

                return 0;
            }
            set
            {
                betAmountInput.text = value.ToString();
            }
        }

        public void Initialize(BoardData boardData)
        {
            ChangeState(State.None);
            wagers = new List<Wager>();
            wagerBoxGUIs = new List<NumberOnBetBoardGUI>();
            for(int i = 0; i < boardData.Boxes.Length; i++)
            {
                var data = boardData.Boxes[i];
                GameObject obj = null;

                // Spinner
                if(data.Name.Equals(SpinnerType.SCircle.ToString()))
                {
                    obj = Instantiate(circleSpinnerPrefab, spinnerParent);

                    if(spinner != null) Destroy(spinner.GameObject);
                    spinner = obj.GetComponent<ISpinner>();
                }

                // Wagers
                else
                {
                    obj = Instantiate(wagerBoxPrefab, wagerBoxParent);

                    var guiObject = obj.GetComponent<NumberOnBetBoardGUI>();
                    guiObject.Background.color = Extensions.StringToColor(data.Color);

                    var sprite = ServiceLocator.GetService<AssetService>().GetSprite(data.Sprite);
                    if(sprite) 
                    {
                        guiObject.Background.sprite = sprite;
                        guiObject.Text.gameObject.SetActive(false);
                    } 
                    else 
                    {
                        guiObject.Text.text = data.VisualText;
                        guiObject.Text.gameObject.SetActive(true);
                    }

                    AddWagerHandler(guiObject, data);
                    guiObject.SetBetAmount(0);

                    wagerBoxGUIs.Add(guiObject);
                }

                var rectTrans = obj.transform as RectTransform;
                rectTrans.anchoredPosition = data.Position;
                rectTrans.sizeDelta = data.Size;
            }

            if(spinner != null)
            {
                spinner.Initialize(boardData.SpinnerConfig);
            }

        }

        public void Play()
        {
            betAmount = startBetAmount;
            ChangeState(State.Betting);
        }

        private void AddWagerHandler(NumberOnBetBoardGUI guiObject, GUIObjectData data)
        {
            guiObject.Button.OnClick.AddListener(eventData =>
            {
                if(blockBetting) return;

                float bonusRate = data.FloatParam;
                WagerType wagerType = Extensions.StringToEnum<WagerType>(data.Name);
                Wager wager = null;

                switch(wagerType)
                {
                    case WagerType.Single:
                        int betNumber = int.Parse(data.StrParam);

                        // Find single wager has same bet number
                        wager = GetSingleWager(betNumber);
                        if(wager == null)
                        {
                            wager = new SingleNumberWager(0, bonusRate, betNumber);
                            wagers.Add(wager);
                        }

                        break;

                    case WagerType.Range:
                        var p = data.StrParam.Split('-');  // expect logic is [from]-[to], example: 1-10
                        int from = int.Parse(p[0]);
                        int to = int.Parse(p[1]);

                        // Find wager has same range
                        wager = GetRangeWager(from, to);
                        if(wager == null)
                        {
                            wager = new RangeWager(0, bonusRate, from, to);
                            wagers.Add(wager);
                        }

                        break;

                    case WagerType.Odd:
                        // Find wager has exist
                        wager = GetOddWager();
                        if(wager == null)
                        {
                            wager = new OddNumberWager(0, bonusRate);
                            wagers.Add(wager);
                        }

                        break;

                    case WagerType.Even:
                        // Find wager has exist
                        wager = GetEvenWager();
                        if(wager == null)
                        {
                            wager = new EvenNumberWager(0, bonusRate);
                            wagers.Add(wager);
                        }

                        break;

                    case WagerType.Color:
                        string colorStr = data.StrParam;

                        // Find wager has same color
                        wager = GetColorWager(colorStr);
                        if(wager == null)
                        {
                            wager = new ColorWager(0, bonusRate, colorStr);
                            wagers.Add(wager);
                        }

                        break;
                }

                bool isAddCurrency = eventData.button == PointerEventData.InputButton.Left;
                bool isWithdrawCurrency = eventData.button == PointerEventData.InputButton.Right;

                if(isAddCurrency) AddCurrencyToWager(wager, guiObject);
                if(isWithdrawCurrency) WithdrawCurrencyFromWager(wager, guiObject);
            });
        }

        private void AddCurrencyToWager(Wager wager, NumberOnBetBoardGUI guiObject)
        {
            if(!player.Bet(betAmount))
            {
                // Not enough currency
                return;
            }

            wager.BetAmount += betAmount;
            guiObject.SetBetAmount(wager.BetAmount);
        }

        private void WithdrawCurrencyFromWager(Wager wager, NumberOnBetBoardGUI guiObject)
        {
            int withdrawAmount = wager.BetAmount < betAmount ? wager.BetAmount : betAmount;
            wager.BetAmount -= withdrawAmount;
            player.Withdraw(withdrawAmount);
            guiObject.SetBetAmount(wager.BetAmount);
        }

        private Wager GetSingleWager(int betNumber)
        {
            foreach(var e in wagers)
            {
                if(e.WagerType != WagerType.Single || (e as SingleNumberWager).BetNumber != betNumber) continue;
                return e;
            }

            return null;
        }

        private Wager GetRangeWager(int from, int to)
        {
            foreach(var e in wagers)
            {
                if(e.WagerType != WagerType.Range) continue;

                var range = e as RangeWager;
                if(from != range.From || to != range.To) continue;

                return e;
            }

            return null;
        }

        private Wager GetOddWager()
        {
            foreach(var e in wagers)
            {
                if(e.WagerType == WagerType.Odd) 
                    return e;
            }

            return null;
        }

        private Wager GetEvenWager()
        {
            foreach(var e in wagers)
            {
                if(e.WagerType == WagerType.Even) 
                    return e;
            }

            return null;
        }

        private Wager GetColorWager(string colorStr)
        {
            foreach(var e in wagers)
            {
                if(e.WagerType != WagerType.Color) continue;

                var color = e as ColorWager;
                if(!color.ColorInString.Equals(colorStr)) continue;

                return e;
            }

            return null;
        }

        private void ChangeState(State newState)
        {
            if(state != null) 
                state.Exit();

            switch(newState)
            {
                case State.Betting:
                    state = bettingState;
                    break;
                case State.Spinning:
                    state = spinningState;
                    break;
                case State.Result:
                    state = resultState;
                    break;
                default:
                    state = null;
                    break;
            }

            if(state != null)
            {
                state.controller = this;
                state.Enter();
            }
        }
        
        private void Spin()
        {
            ChangeState(State.Spinning);
        }

        private void CloseResult()
        {
            ChangeState(State.Betting);
        }

        void Awake()
        {
            playBoardManager = GetComponent<PlayBoardManager>();
            spinBtn.onClick.AddListener(Spin);
            closeResultBtn.onClick.AddListener(CloseResult);
        }

        void Update()
        {
            if(state != null) state.Update();
        }
    
#region States
        private static Betting bettingState = new Betting();
        private static Spinning spinningState = new Spinning();
        private static Result resultState = new Result();

        enum State
        {
            None, Betting, Spinning, Result
        }

        interface IState
        {
            PlayBoardController controller {get; set;}
            void Enter();
            void Update();
            void Exit();
        }

        class Betting : IState
        {
            public PlayBoardController controller {get; set;}
            public int LastestTotalBetAmount {get; private set;}

            public void Enter()
            {
                controller.spinBtn.gameObject.SetActive(true);
                controller.spinnerParent.gameObject.SetActive(false);
                controller.blockBetting = false;

                // Reset UI
                foreach(var e in controller.wagerBoxGUIs)
                    e.SetBetAmount(0); 
            }

            public void Update()
            {
                controller.playerCurrencyTxt.text = controller.player.CurrencyCount.ToString();

                int totalBetAmount = 0;
                foreach(var e in controller.wagers)
                    totalBetAmount += e.BetAmount;
                controller.totalBetAmountTxt.text = totalBetAmount.ToString(); 
                LastestTotalBetAmount = totalBetAmount;
            }

            public void Exit()
            {
            }
        }

        class Spinning : IState
        {
            public PlayBoardController controller {get; set;}
            public int LastesReward {get; private set;}

            private Coroutine handleSpin;
            private BettingHistory history => ServiceLocator.GetService<BettingHistory>();

            public void Enter()
            {
                controller.spinBtn.gameObject.SetActive(false);
                controller.spinnerParent.gameObject.SetActive(true);
                controller.blockBetting = true;
                handleSpin = controller.StartCoroutine(HandleSpin());
            }

            public void Update()
            {
            }

            public void Exit()
            {
                if(handleSpin != null)
                {
                    controller.StopCoroutine(handleSpin);
                }
            }       

            private IEnumerator HandleSpin()
            {
                // Test result
                BettingResult expectResult = new BettingResult
                {
                    Number = 10,
                    Color = "0,0,0,1"
                };

                history.Add(expectResult);
                CheckRewardForPlayer();
                controller.spinner.Spin(expectResult.Number);

                yield return new WaitUntil(()=>!controller.spinner.IsSpinning);
                yield return new WaitForSeconds(1f);

                controller.ChangeState(State.Result);
            } 

            private void CheckRewardForPlayer()
            {
                var player = controller.player;
                var wagers = controller.wagers;
                int cachePlayerCurrency = player.CurrencyCount;

                foreach(var wager in wagers)
                {
                    if(wager.IsRewardAble())
                    {
                        wager.Reward(player);
                    }
                }

                LastesReward = player.CurrencyCount - cachePlayerCurrency;

                wagers.Clear();
            }
        }

        class Result : IState
        {
            public PlayBoardController controller {get; set;}

            public void Enter()
            {
                controller.playerCurrencyTxt.text = controller.player.CurrencyCount.ToString();
                controller.resultLayout.SetActive(true);
                controller.resultBetAmountTxt.text  = $"Bet:    {bettingState.LastestTotalBetAmount.ToString()}";
                controller.resultRewardTxt.text     = $"Reward: {spinningState.LastesReward.ToString()}";

                Debug.Log($"Player bet {bettingState.LastestTotalBetAmount} and won {spinningState.LastesReward}");
            }

            public void Update()
            {
            }

            public void Exit()
            {
                controller.resultLayout.SetActive(false);
            }
        }
        #endregion
    }
}
