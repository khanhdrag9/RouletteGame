using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.PlayBoard
{
    public class PlayBoardManager : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Mode mode;

        [Header("GUI")]
        [SerializeField] private Button spinButton;


        private ModeData GetModeData()
        {
            ModeData result = new ModeData
            {
                numbers = new int[]
                {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19}
            };

            return result;
        } 

        void Start()
        {
            mode.InitalizeBoard(GetModeData());
            spinButton.onClick.AddListener(Spin);
        }

        void Update()
        {
            
        }

        public void Spin()
        {

        }
    }
}
