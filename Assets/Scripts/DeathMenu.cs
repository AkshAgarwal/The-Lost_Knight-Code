using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMenu : MonoBehaviour
{
   
    // Start is called before the first frame update
    public void Reset()
    {
        SceneManager.LoadScene("Level1");
        GameObject menu = GameObject.FindGameObjectWithTag("DeathMenu");
        menu.SetActive(false);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void quit()
    {
        Application.Quit();
    }
}

