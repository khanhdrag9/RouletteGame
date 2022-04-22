using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Design
{
    public class WagerDesign : GuiDesignObject
    {
        [SerializeField] public float BonusRate;
        public virtual string LogicInStr => GetComponentInChildren<Text>().text;  
    }
}
