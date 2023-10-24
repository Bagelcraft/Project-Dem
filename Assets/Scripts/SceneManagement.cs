using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement: MonoBehaviour
{
    public void LoadGame1()
    {
        // Load the game scene (replace "GameSceneName" with the actual scene name)
        SceneManager.LoadScene("Game 2 Food");
    }

    public void LoadGame2()
    {
        // Load the game scene (replace "GameSceneName" with the actual scene name)
        SceneManager.LoadScene("Game 3");
    }

    public void QuitGame()
    {
        // Quit the game (only works in standalone builds)
        Application.Quit();
    }
}
