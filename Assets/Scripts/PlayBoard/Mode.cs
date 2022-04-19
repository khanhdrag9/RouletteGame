using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Bet;
namespace Game
{
    public class Mode : MonoBehaviour
    {
        [Header("Spinner")]
        [SerializeField] private RectTransform spinnerElementGroup;
        [SerializeField] private GameObject spinnerElementPrefab;

        [Header("Bet Option")]
        [SerializeField] private RectTransform betOptionGroup;
        [SerializeField] private GameObject betOptionPrefab;

        public void Initalize(ModeData modeData)
        {
            InitializeNumberSpinner(modeData.Numbers);
            InitalizeBetOptions(modeData.AvailableBet);
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
            for(int i = 0; i < betDatas.Length; i++)
            {
                BetData betData = betDatas[i];

                GameObject betOption = SpawnBetOption();
                BetOptionGUI betOptionGUI = betOption.GetComponent<BetOptionGUI>();
                
                betOptionGUI.BetBtn.onClick.AddListener(()=>
                {
                    // Handle Creating a bet
                    IBetCreator creator = null;
                    string defineName = betData.Name.ToLower();
                    switch(defineName)
                    {
                        case "singlebet":
                            creator = new SingleBetCreator(betOptionGUI.BetAmount);
                            break;
                    }
                });

                betOptionGUI.TextVisual.text = betData.Visual;
            }
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
