using System.Collections;
using System.Collections.Generic;
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
        private List<WagerBase> wagers;
        private int unit => 1;

        public void Initialize(BoardData boardData)
        {
            wagers = new List<WagerBase>();
            for(int i = 0; i < boardData.Boxes.Length; i++)
            {
                var data = boardData.Boxes[i];

                var box = Instantiate(wagerBoxPrefab, wagerBoxParent);

                var transform = box.transform as RectTransform;
                transform.position = data.Position;
                transform.sizeDelta = data.Size;

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
                WagerBase wager = null;

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
                        break;
                }

            });
        }

        private void AddCurrencyToWager(WagerBase wager)
        {
            wager.BetAmount += unit;
        }

        void Awake()
        {
            playBoardManager = GetComponent<PlayBoardManager>();
        }
    }
}
