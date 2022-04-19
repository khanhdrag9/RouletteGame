using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BetOptionGUI : MonoBehaviour
    {
        [SerializeField] private Button betBtn;
        [SerializeField] private Text textVisual;
        [SerializeField] private InputField betInput;

        public Button BetBtn => betBtn;
        public Text TextVisual => textVisual;
        public InputField BetInput => betInput;

        public int BetNumber
        {
            get 
            {
                if(int.TryParse(betInput.text, out int amount))
                    return amount;
                
                return -1;
            }
        }

        public void Active(bool active)
        {
            betBtn.interactable = active;
            betInput.interactable = active;
        }
    }
}
