using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public Bankroll Bankroll {get; private set;}

        private void Awake()
        {
            Bankroll = new Bankroll();
        }
    }
}
