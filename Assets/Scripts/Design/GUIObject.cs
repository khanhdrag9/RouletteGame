using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Design
{
    public enum GUIObjectType
    {
        None,
        SingleWager,
        RangeWager,
        OddWager,
        EvenWager,
        ColorWager
    }

    public class GUIObject : MonoBehaviour
    {
        [SerializeField] public GUIObjectType Type;
    }
}
