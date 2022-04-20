using System.Collections;
using System.Collections.Generic;
using Game.Asset;
using Game.Bet;
using Game.Helper;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class PlayBoardController : MonoBehaviour
    {
        [SerializeField] private GameObject wagerBoxPrefab;
        [SerializeField] private RectTransform wagerBoxParent;

        private PlayBoardManager playBoardManager;
        private List<Wager> wagers;
        private int unit => 1;

        public void Initialize(BoardData boardData)
        {
            wagers = new List<Wager>();
            for(int i = 0; i < boardData.Boxes.Length; i++)
            {
                var data = boardData.Boxes[i];

                var box = Instantiate(wagerBoxPrefab, wagerBoxParent);

                var rectTrans = box.transform as RectTransform;
                rectTrans.ForceUpdateRectTransforms();
                Debug.Log("Set data: " + data.Position);
                rectTrans.anchoredPosition = data.Position;
                rectTrans.sizeDelta = data.Size;

                var guiObject = box.GetComponent<NumberOnBetBoardGUI>();
                guiObject.Background.color = Extensions.StringToColor(data.Color);
                guiObject.Text.text = data.VisualText;
                AddHandler(guiObject, data);
            }
        }

        private void AddHandler(NumberOnBetBoardGUI guiObject, WagerBox data)
        {
            guiObject.Button.onClick.AddListener(()=>
            {
                float bonusRate = 1.5f;
                WagerType wagerType = Extensions.StringToEnum<WagerType>(data.Name);
                Wager wager = null;

                switch(wagerType)
                {
                    case WagerType.Single:
                        int betNumber = int.Parse(data.Logic);

                        // Find single wager has same bet number
                        foreach(var e in wagers)
                        {
                            if(e.WagerType != WagerType.Single || (e as SingleNumberWager).BetNumber != betNumber) continue;
                            wager = e;
                            break;
                        }

                        if(wager == null)
                        {
                            wager = new SingleNumberWager(0, bonusRate, betNumber);
                            wagers.Add(wager);
                        }

                        AddCurrencyToWager(wager);
                        break;

                    case WagerType.Range:
                        var p = data.Logic.Split('-');  // expect logic is [from]-[to], example: 1-10
                        int from = int.Parse(p[0]);
                        int to = int.Parse(p[1]);

                        // Find wager has same range
                        foreach(var e in wagers)
                        {
                            if(e.WagerType != WagerType.Range) continue;

                            var range = e as RangeWager;
                            if(from != range.From || to != range.To) continue;

                            wager = e;
                            break;
                        }

                        if(wager == null)
                        {
                            wager = new RangeWager(0, bonusRate, from, to);
                            wagers.Add(wager);
                        }

                        AddCurrencyToWager(wager);
                        break;

                    case WagerType.Odd:
                        // Find wager has exist
                        foreach(var e in wagers)
                        {
                            if(e.WagerType != WagerType.Odd) continue;
                            wager = e;
                            break;
                        }

                        if(wager == null)
                        {
                            wager = new OddNumberWager(0, bonusRate);
                            wagers.Add(wager);
                        }

                        AddCurrencyToWager(wager);
                        break;

                    case WagerType.Even:
                        // Find wager has exist
                        foreach(var e in wagers)
                        {
                            if(e.WagerType != WagerType.Even) continue;
                            wager = e;
                            break;
                        }

                        if(wager == null)
                        {
                            wager = new EvenNumberWager(0, bonusRate);
                            wagers.Add(wager);
                        }

                        AddCurrencyToWager(wager);
                        break;

                    case WagerType.Color:
                        string colorStr = data.Logic;

                        // Find wager has same color
                        foreach(var e in wagers)
                        {
                            if(e.WagerType != WagerType.Color) continue;

                            var color = e as ColorWager;
                            if(!color.ColorInString.Equals(colorStr)) continue;

                            wager = e;
                            break;
                        }

                        if(wager == null)
                        {
                            wager = new ColorWager(0, bonusRate, colorStr);
                            wagers.Add(wager);
                        }

                        AddCurrencyToWager(wager);   
                        break;
                }

            });
        }

        private void AddCurrencyToWager(Wager wager)
        {
            wager.BetAmount += unit;
        }

        void Awake()
        {
            playBoardManager = GetComponent<PlayBoardManager>();
        }
    }
}
