using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class ListBoardData
    {
        public BoardData[] Value;
    }

    /// <summary>
    /// Design data of a gameplay
    /// </summary>
    [System.Serializable]
    public class BoardData
    {
        /// <summary>
        /// Game play name
        /// </summary>
        public string Name;

        /// <summary>
        /// gameplay is designed based on this resolution
        /// </summary>
        public Vector2 DesignResolution;

        /// <summary>
        /// Array data of objects which is used to place wager
        /// </summary>
        public GUIObjectData[] Boxes;
        public SpinnerConfig SpinnerConfig;
    }

    /// <summary>
    /// Data used to create UI object which help player to place wager
    /// </summary>
    [System.Serializable]
    public class GUIObjectData
    {
        /// <summary>
        /// Name of object
        /// </summary>
        public string Name;

        /// <summary>
        /// Position of a object. 
        /// Use for RectTransform.anchorPosition when it is an UI object, anchorPosition is based on center anchor preset
        /// </summary>
        public Vector2 Position;    

        /// <summary>
        /// Size of a object. 
        /// Use for RectTransform.sizeDelta when it is an UI object
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// Color of Image component
        /// </summary>
        public string Color;

        /// <summary>
        /// String value is used in Text component
        /// </summary>
        public string VisualText;

        /// <summary>
        /// Sprite is used in Image component
        /// </summary>
        public string Sprite;

        /// <summary>
        /// float parameter, used for multi purposes like bonusRate for wager design
        /// </summary>
        public float FloatParam;

        /// <summary>
        /// String parameter, used for multi purposes
        /// </summary>
        public string StrParam;
    }

    /// <summary>
    /// Data used to create UI object which help player to place wager
    /// </summary>
    [System.Serializable]
    public class SpinnerConfig
    {
        public SpinnerItem[] Items;
    }

    [System.Serializable]
    public class SpinnerItem
    {
        public int Number;
        public string Color;
        public string Sprite;
    }
}
