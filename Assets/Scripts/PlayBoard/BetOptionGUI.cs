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

        public int BetAmount
        {
            get 
            {
                if(int.TryParse(betInput.text, out int amount))
                    return amount;
                
                return 0;
            }
        }
    }
}
