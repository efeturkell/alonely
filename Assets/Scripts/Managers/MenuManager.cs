using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject Panel;
    private bool optionsMenuActive = false;

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionsMenuActive)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && optionsMenuActive)
        {
            HideOptions();
        }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
        
    }

    public void StopButton()
    {
        Application.Quit();
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Panel.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Panel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    

    public void ShowOptions()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        gameIsPaused = true;
        optionsMenuActive = true;
    }

    public void HideOptions()
    {
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
        optionsMenuActive = false;
    }

    public void SetQuality(int qual)
    {
        QualitySettings.SetQualityLevel(qual);
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    public void SetMusic(bool isMusic)
    {
        AudioManager.instance.GameMusic.mute = !isMusic;
       

    }
}
