using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public abstract class BoardStateBase : MonoBehaviour
    {
        [SerializeField] protected PlayBoardManager playBoardManager;
    }
}
