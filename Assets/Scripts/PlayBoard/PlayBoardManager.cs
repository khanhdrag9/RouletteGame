using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayBoardManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private PlayBoardController playBoardController;

        public Player Player => player;

        void Start()
        {
            playBoardController.Initialize(Global.BoardData);
            playBoardController.Play();
        }
    }
}
