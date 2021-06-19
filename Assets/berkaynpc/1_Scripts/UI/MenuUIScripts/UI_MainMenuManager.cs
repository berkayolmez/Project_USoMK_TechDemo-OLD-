using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace project_usomk
{
    public class UI_MainMenuManager : MonoBehaviour     //This will be changed with observer pattern 
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private bool isMenuOpen;
        //public List<string> targetID = new List<string>();
        [SerializeField] private GameObject[] closeObjs;

        private void Update()
        {
            if(Keyboard.current.escapeKey.wasPressedThisFrame)      //Show/Hide Main Menu
            {
                isMenuOpen = !isMenuOpen;

                if (isMenuOpen)
                {
                    mainMenu.SetActive(true);
                }
                else
                {
                    mainMenu.SetActive(false);
                    //MyGameEvents.current.SetTarget(targetID, false);                    
                    if (closeObjs.Length>0)     //Close tabs, images etc. //This will be changed with observer pattern
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