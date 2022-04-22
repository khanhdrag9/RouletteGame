using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // Player currency should be saved on the server/datebase
    // Bankroll will be used to send request get/update currency of player
    // For now, We will handle locally for Demo
    public class Bankroll
    {
        private List<Currency> currencies;

        public int CurrencyCount => currencies.Count;

        public Bankroll()
        {
            currencies = new List<Currency>();
            
            // Start with 1000
            Receive(1000);
        }

        public void Spend(int number)
        {
            currencies.RemoveRange(0, number);
        }   

        public void Receive(int number)
        {
            Currency[] bonus = new Currency[number];
            for(int i = 0; i < bonus.Length; i++)
            {
                bonus[i] = new Currency();
            }

            currencies.AddRange(bonus);
        }     
    }
}
