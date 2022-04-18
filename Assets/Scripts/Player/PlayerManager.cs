using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private Bankroll bankroll;
        private BetFactory betFactory;
        private List<IBet> bets;

        public void Bet(BetType betType, int numberCurrencies)
        {
            bankroll.Spend(numberCurrencies);
            IBet bet = betFactory.GetBet(betType);
            bets.Add(bet);
        }

        public void CheckReward()
        {
            foreach(var bet in bets)
            {
                if(bet.IsRewardAble())
                {
                    bet.Reward();
                }
            }
        }

        private void Initialize()
        {
            bankroll    = new Bankroll();
            betFactory  = new BetFactory();
            bets        = new List<IBet>();
        }


#region Unity method
        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {

        }
#endregion

    }
}
