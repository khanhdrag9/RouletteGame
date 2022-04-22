using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MenuEditor
{
    [MenuItem("Scenes/Play")]
    private static void Play()
    {
        OpenMainMenuScene();
        EditorApplication.isPlaying = true;
    }

    [MenuItem("Scenes/Main menu scene")]
    private static void OpenMainMenuScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    [MenuItem("Scenes/Game play scene")]
    private static void OpenGameplayScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Gameplay.unity");
    }

    [MenuItem("Scenes/Design scene")]
    private static void OpenDesignScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/BoardLayoutDesign.unity");
    }
}