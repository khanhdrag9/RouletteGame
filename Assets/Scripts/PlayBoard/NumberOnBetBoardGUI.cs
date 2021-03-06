using Game.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class NumberOnBetBoardGUI : MonoBehaviour
    {
        [SerializeField] public Image Background;
        [SerializeField] public ButtonExtension Button;
        [SerializeField] public Text Text;
        [SerializeField] private GameObject betCoinIcon;
        [SerializeField] private Text betCoinTxt;

        public void SetBetAmount(int amount)
        {
            betCoinTxt.text = amount.ToString();
            betCoinIcon.SetActive(amount > 0);
        }
    }
}
