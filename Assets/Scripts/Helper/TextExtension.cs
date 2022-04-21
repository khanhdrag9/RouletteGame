using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Text))]
    public class TextExtension : MonoBehaviour
    {
        private Text textCom;
        
        public string TextValue {get; set;}

        public void UpdateVisual()
        {
            textCom.text = TextValue;
        }

        void Awake()
        {
            textCom = GetComponent<Text>();
        }
    }
}
