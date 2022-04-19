using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class NumberOnBetBoardGUI : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Button button;
        [SerializeField] private Text numberTxt;

        public void Initialize(int number, Color color, UnityAction clickCallback)
        {
            numberTxt.text = number.ToString();
            background.color = color;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickCallback);
        }
    }
}
