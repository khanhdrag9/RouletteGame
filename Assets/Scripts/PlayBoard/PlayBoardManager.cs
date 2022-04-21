using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Helper;
using Game.Asset;

namespace Game
{
    public enum BoardState
    {
        None,
        Betting,
        Spinning
    }

    public class PlayBoardManager : MonoBehaviour
    {
        public static PlayBoardManager Instance {get; private set;}


        [Header("General")]
        [SerializeField] private Player player;
        [SerializeField] private PlayBoardController playBoardController;

        public Player Player => player;


#region Unity methods
        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            BoardData[] boardDatas = ServiceLocator.GetService<AssetService>().GetBoardData();
            playBoardController.Initialize(boardDatas[0]);
            playBoardController.Play();
        }

        void Update()
        {
        }
#endregion

    }
}
