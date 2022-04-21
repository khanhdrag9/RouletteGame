using UnityEngine.SceneManagement;

namespace Game.Helper
{
    public static class SceneLoader
    {
        public static void GoMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public static void GoPlay()
        {
            SceneManager.LoadScene(1);
        }
    }
}