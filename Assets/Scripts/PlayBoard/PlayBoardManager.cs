using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayBoardManager : MonoBehaviour
    {
        public static PlayBoardManager Instance {get; private set;}


        [Header("General")]
        [SerializeField] private Player player;
        [SerializeField] private Mode mode;

        [Header("GUI")]
        [SerializeField] private Button spinButton;
        [SerializeField] private Text playerCurrency;

        [Header("Debug")]
        [SerializeField] private int debugSpinResult = 0;

        private List<IBet> bets;

        public int Result {get; private set;} = -1;
        

        private ModeData GetModeData()
        {
            ModeData dataResult = new ModeData
            {
                numbers = new int[]
                {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19}
            };

            return dataResult;
        } 

        private void Spin()
        {
            // Test
            bets.Add(new SingleBet(0, 100));

            StartCoroutine(WaitSpinning());
        }

        private IEnumerator WaitSpinning()
        {
            Result = debugSpinResult;
            yield return new WaitForSeconds(3f);

            CheckRewardOfPlayer();
        }

        private void CheckRewardOfPlayer()
        {
            foreach(var bet in bets)
            {
                if(bet.IsRewardAble())
                {
                    bet.Reward(player);
                }
            }

            bets.Clear();
        }

#region Unity methods
        void Awake()
        {
            Instance = this;
            bets = new List<IBet>();
        }

        void Start()
        {
            mode.InitalizeBoard(GetModeData());
            spinButton.onClick.AddListener(Spin);
        }

        void Update()
        {
            playerCurrency.text = player.Bankroll.CurrencyCount.ToString();
        }
#endregion

    }
}
