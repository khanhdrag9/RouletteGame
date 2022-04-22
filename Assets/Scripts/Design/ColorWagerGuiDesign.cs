using System.Collections;
using System.Collections.Generic;
using Game.Helper;
using UnityEngine;

namespace Design
{
    public class ColorWagerGuiDesign : WagerDesign
    {
        [SerializeField] public Color color;
        public override string LogicInStr => $"{Extensions.ColorToString(color)}";
    }
}
