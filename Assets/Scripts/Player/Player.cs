using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        private Bankroll bankroll;

        public int CurrencyCount => bankroll.CurrencyCount;

        public bool Bet(int betAmount)
        {
            if(bankroll.CurrencyCount >= betAmount)
            {
                bankroll.Spend(betAmount);
                return true;
            }

            return false;
        }

        public void Reward(int reward)
        {
            bankroll.Receive(reward);
        }

        private void Awake()
        {
            bankroll = new Bankroll();
        }
    }
}
