using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Bankroll
    {
        private List<Currency> currencies;

        public int CurrencyCount => currencies.Count;

        public Bankroll()
        {
            currencies = new List<Currency>();
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
