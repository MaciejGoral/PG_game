using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menumanager : MonoBehaviour
{
    // Start is called before the first frame update
    public void startGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void exitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void exitGame2()
    {
        Application.Quit();
    }
}
