using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Design
{
    public class SpinnerDesign : GuiDesignObject
    {
        [SerializeField] public int[] Order;
        [SerializeField] public Color[] Colors;
        [SerializeField] public Sprite[] Sprites;
    }
}
