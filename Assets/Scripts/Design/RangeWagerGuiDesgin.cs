using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Design
{
    public class RangeWagerGuiDesgin : WagerDesign
    {
        [SerializeField] public int From;
        [SerializeField] public int To;
        public override string LogicInStr => $"{From}-{To}";
        
    }
}
