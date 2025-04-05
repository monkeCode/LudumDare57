using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.PauseMenu
{
    public class PauseMenuController : MonoBehaviour
    {
        public GameObject pauseMenuCanvas;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeState();
            }
        }

        public void ChangeState()
        {
            if (pauseMenuCanvas.activeSelf)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Open()
        {
            Time.timeScale = 0;
            pauseMenuCanvas.SetActive(true);
        }
        

        public void Close()
        {
            Time.timeScale = 1;
            pauseMenuCanvas.SetActive(false);
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
