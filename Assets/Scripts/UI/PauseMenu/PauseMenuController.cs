using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.PauseMenu
{
    public class PauseMenuController : MonoBehaviour
    {
        public GameObject pauseMenuCanvas;

        public static PauseMenuController Instance { get; private set; }

        public bool Paused => pauseMenuCanvas.activeSelf;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

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
                Player.Player.Instance.Inputs.Player.Enable();

            }
            else
            {
                Open();
                Player.Player.Instance.Inputs.Player.Disable();
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
