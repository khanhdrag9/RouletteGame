using UnityEngine;

namespace Game
{
    public interface ISpinner
    {
        GameObject GameObject {get;}
        bool InProgress {get;}
        void Initialize(int[] orderOfNumer);
        void Spin(int expectResult);
    }
}