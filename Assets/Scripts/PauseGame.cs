using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public bool gameActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetPause();
        }
    }

    public void SetPause() 
    {
        gameActive = !gameActive;
        pauseMenu.SetActive(!gameActive);
        if (gameActive)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
        SetPause();
        /*
        pauseMenu.SetActive(false);//por si acaso
        Time.timeScale = 1;//por si acaso
        gameActive = true;
        */
    }
}
