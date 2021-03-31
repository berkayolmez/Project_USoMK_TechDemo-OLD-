using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace project_WAST
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private bool isMenuOpen;

        private void Start()
        {
            isMenuOpen = true;
        }

        private void Update()
        {
            if(Keyboard.current.escapeKey.wasPressedThisFrame) //game pad ekle
            {
                isMenuOpen = !isMenuOpen;

                if (isMenuOpen)
                {
                    mainMenu.SetActive(true);
                }
                else
                {
                    mainMenu.SetActive(false);
                }
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void CloseTab()
        {
            isMenuOpen = false;
        }

    }
}