using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    public GameObject panel;
    public GameObject optionsPanel;
    public Slider slider;
    public GameObject HowToPlayPanel;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = AudioListener.volume;

        slider.onValueChanged.AddListener(UpdateVolume);
    }


    void UpdateVolume(float value)
    {
        // Set the audio listener volume to the slider value
        AudioListener.volume = value;
    }
void Update()
    {
        // Check if the user presses Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If both canvases are disabled, enable only the pause panel and pause the game
            if (!panel.activeSelf)
            {
                panel.SetActive(true);
                optionsPanel.SetActive(false);
                Time.timeScale = 0f;
            }
            else if (panel.activeSelf)
            {
                panel.SetActive(false);
                optionsPanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
    public void resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void options()
    {
        optionsPanel.SetActive(true);
    }

    public void back()
    {
        optionsPanel.SetActive(false);
    }

    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log(Screen.fullScreen);
    }

    public void howToPlay()
    {
        HowToPlayPanel.SetActive(true);
    }

    public void howToPlayBack()
    {
        HowToPlayPanel.SetActive(false);
    }

}
