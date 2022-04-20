using System.Collections;
using System.Collections.Generic;
using Game.Bet;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BettingState : BoardStateBase
    {
        [SerializeField] private Button spinButton;

        private Player player => playBoardManager.Player;

        private void Spin()
        {
            playBoardManager.BoardState = BoardState.Spinning;
        }


#region Unity methods
        private void Awake()
        {
            spinButton.onClick.AddListener(Spin);
        }

        private void OnEnable() 
        {
            spinButton.gameObject.SetActive(true);
        }

        private void OnDisable() 
        {
            if(spinButton) spinButton.gameObject.SetActive(false);
        }

#endregion

    }
}
