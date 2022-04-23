using Game.Data;
using UnityEngine;

namespace Game
{
    public interface ISpinner
    {
        GameObject GameObject {get;}
        bool IsSpinning {get;}
        void Initialize(SpinnerConfig config);
        void Spin(int expectResult);
    }
}