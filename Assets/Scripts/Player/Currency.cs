using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Currency
    {
        public int Value {get; private set;} = 1;

        public Currency(int baseValue = 1)
        {
            Value = baseValue;
        }
    }
}
