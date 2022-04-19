using System.Collections;
using System.Collections.Generic;
using Game.Bet;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class BoardData
    {
        public Vector2 DesignResolution;
        public WagerBox[] Boxes;
        public int[] Numbers;
        public BetData[] AvailableBet;
    }

    [System.Serializable]
    public class WagerBox
    {
        public string Name;
        public Vector2 Position;
        public Vector2 Size;
        public string Color;
        public string VisualText;
        public string Logic;
    }
}
