using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Helper
{
    public class SceneLoader : MonoBehaviour
    {
        public void GoMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void GoPlay()
        {
            SceneManager.LoadScene(1);
        }
    }
}