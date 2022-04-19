using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Bet;
using UnityEngine.Events;

namespace Game
{
    public class ModeController : MonoBehaviour
    {
        [Header("Spinner")]
        [SerializeField] private RectTransform spinnerElementGroup;
        [SerializeField] private GameObject spinnerElementPrefab;

        [Header("Bet Option")]
        [SerializeField] private RectTransform betOptionGroup;
        [SerializeField] private GameObject betOptionPrefab;
        [SerializeField] private InputField betAmountInput;


        private List<BetOptionGUI> betOptionGUIs = new List<BetOptionGUI>();
        private List<WagerBase> bets = new List<WagerBase>();
        private int betAmount
        {
            get 
            {
                if(int.TryParse(betAmountInput.text, out int amount))
                    return amount;
                
                return -1;
            }
        }

        public Player Player {get; set;}

        public void SetBetable(bool active)
        {
            foreach(var e in betOptionGUIs)
            {
                e.Active(active);
            }
        }

        public void Initalize(BoardData modeData)
        {
            InitializeNumberSpinner(modeData.Numbers);
            InitalizeBetOptions(modeData.AvailableBet);
        }

        public void CheckRewardOfPlayer()
        {
            foreach(var bet in bets)
            {
                if(bet.IsRewardAble())
                {
                    bet.Reward(Player);
                }
            }

            bets.Clear();
        }

        private void InitializeNumberSpinner(int[] orderOfNumer)
        {
            int numberElement = orderOfNumer.Length;
            for(int i = 0; i < numberElement; ++i)
            {
                GameObject element = SpawnElementInSpinner();

                // Adjust angle of number element in spinner board
                Image eImage = element.GetComponent<Image>();
                eImage.color = i % 2 == 0 ? Color.red : Color.black;
                eImage.fillAmount = 1f / numberElement;
                eImage.rectTransform.localRotation = Quaternion.Euler(0, 0, (i + 0.5f) * 360f / numberElement);
                eImage.rectTransform.sizeDelta = spinnerElementGroup.sizeDelta;

                // Adjust position of number text in each element
                Text eText = element.GetComponentInChildren<Text>();
                eText.text = orderOfNumer[i].ToString();
                eText.rectTransform.localRotation = Quaternion.Euler(0, 0, -180f / numberElement);
            }
        }

        private void InitalizeBetOptions(BetData[] betDatas)
        {
            betOptionGUIs = new List<BetOptionGUI>();
            for(int i = 0; i < betDatas.Length; i++)
            {
                BetData betData = betDatas[i];

                GameObject betOption = SpawnBetOption();
                BetOptionGUI betOptionGUI = betOption.GetComponent<BetOptionGUI>();

                HandleBetOption(betOptionGUI, betData);
                betOptionGUI.TextVisual.text = betData.Visual;

                betOptionGUIs.Add(betOptionGUI);
            }
        }

        private void HandleBetOption(BetOptionGUI betOptionGUI, BetData betData)
        {
            string defineName = betData.Name.ToLower();

            // Setup GUI
            switch(defineName)
            {
                case Constants.BetSingleNumber:
                    betOptionGUI.BetInput.gameObject.SetActive(true);
                    break;
                default:
                    betOptionGUI.BetInput.gameObject.SetActive(false);
                    break;
            }

            // Setup handler betting
            betOptionGUI.BetBtn.onClick.AddListener(()=>
            {
                // Is player enough money?
                if(!Player.Bet(betAmount))
                {
                    return;
                }

                WagerBase bet = null;
                switch(defineName)
                {
                    case Constants.BetSingleNumber:
                        int BetNumber = betOptionGUI.BetNumber;

                        // Player has not picked number
                        if(BetNumber < 0) 
                        {
                            break;
                        }

                        bet = new SingleNumberWager(betAmount, betData.BonusRate, BetNumber);
                        
                        break;
                    case Constants.BetEvenNumber:
                        bet = new EvenNumberWager(betAmount, betData.BonusRate);
                        break;
                    case Constants.BetOddNumber:
                        bet = new OddNumberWager(betAmount, betData.BonusRate);
                        break;
                }

                if(bet != null)
                {
                    bets.Add(bet);
                }
            });
        }

        private GameObject SpawnElementInSpinner()
        {
            var element = Instantiate(spinnerElementPrefab, spinnerElementGroup);
            return element;
        }

        private GameObject SpawnBetOption()
        {
            var betOption = Instantiate(betOptionPrefab, betOptionGroup);
            return betOption;
        }
    }
}
