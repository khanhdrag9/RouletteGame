using System.Collections;
using System.Collections.Generic;
using Game.Bet;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SpinningState : BoardStateBase
    {
        private ModeController modeController => playBoardManager.ModeController;

        Coroutine spinningCoroutine;
        private IEnumerator Spinning()
        {
            yield return new WaitForSeconds(3f);

            modeController.CheckRewardOfPlayer();
            playBoardManager.BoardState = BoardState.Betting;
        }

#region Unity methods
        void Awake()
        {
        }

        private void OnEnable() 
        {
            spinningCoroutine = StartCoroutine(Spinning());
        }

        private void OnDisable() 
        {
            StopCoroutine(spinningCoroutine);
        }

#endregion

    }
}
