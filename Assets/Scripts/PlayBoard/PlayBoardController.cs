using System.Collections.Generic;
using Game.Asset;
using Game.Helper;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class PlayBoardController : MonoBehaviour
    {
        [SerializeField] private GameObject wagerBoxPrefab;
        [SerializeField] private RectTransform wagerBoxParent;

        private PlayBoardManager playBoardManager;
        private Player player => playBoardManager.Player;
        private List<NumberOnBetBoardGUI> wagerBoxGUIs;
        private List<Wager> wagers;
        private int unit => 1;

        public void Initialize(BoardData boardData)
        {
            wagers = new List<Wager>();
            wagerBoxGUIs = new List<NumberOnBetBoardGUI>();
            for(int i = 0; i < boardData.Boxes.Length; i++)
            {
                var data = boardData.Boxes[i];

                var box = Instantiate(wagerBoxPrefab, wagerBoxParent);

                var rectTrans = box.transform as RectTransform;
                rectTrans.anchoredPosition = data.Position;
                rectTrans.sizeDelta = data.Size;

                var guiObject = box.GetComponent<NumberOnBetBoardGUI>();
                guiObject.Background.color = Extensions.StringToColor(data.Color);
                guiObject.Text.text = data.VisualText;
                AddHandler(guiObject, data);
                guiObject.SetBetAmount(0);

                wagerBoxGUIs.Add(guiObject);
            }
        }

        private void AddHandler(NumberOnBetBoardGUI guiObject, WagerBox data)
        {
            guiObject.Button.OnClick.AddListener(eventData =>
            {
                float bonusRate = 1.5f;
                WagerType wagerType = Extensions.StringToEnum<WagerType>(data.Name);
                Wager wager = null;

                switch(wagerType)
                {
                    case WagerType.Single:
                        int betNumber = int.Parse(data.Logic);

                        // Find single wager has same bet number
                        wager = GetSingleWager(betNumber);
                        if(wager == null)
                        {
                            wager = new SingleNumberWager(0, bonusRate, betNumber);
                            wagers.Add(wager);
                        }

                        break;

                    case WagerType.Range:
                        var p = data.Logic.Split('-');  // expect logic is [from]-[to], example: 1-10
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
                        string colorStr = data.Logic;

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
            if(!player.Bet(unit))
            {
                // Not enough currency
                return;
            }

            wager.BetAmount += unit;
            guiObject.SetBetAmount(wager.BetAmount);

        }

        private void WithdrawCurrencyFromWager(Wager wager, NumberOnBetBoardGUI guiObject)
        {
            int withdrawAmount = wager.BetAmount < unit ? wager.BetAmount : unit;
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

        void Awake()
        {
            playBoardManager = GetComponent<PlayBoardManager>();
        }

        void Update()
        {

        }
    }
}
