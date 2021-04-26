using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace project_WAST
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private bool isMenuOpen;
        [SerializeField] private GameObject[] closeObjs;
       
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

                    if(closeObjs.Length>0)
                    {
                        for (int i = 0; i < closeObjs.Length; i++)
                        {
                            closeObjs[i].SetActive(false);
                        }
                    }
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