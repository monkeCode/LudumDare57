using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{ 
    public string startScene = "PlayerDemo";
    
    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}