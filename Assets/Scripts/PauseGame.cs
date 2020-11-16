using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState { pauseScreen, endScreen, gameScreen};
public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endScreen;
    public bool gameActive;
    public float maxTimeWaiting;
    public float maxTimeEndMenu;
    float timer;
    public MenuState state;

    private void Start()
    {
        maxTimeWaiting = 5f;
        maxTimeEndMenu = 5f;
        timer = maxTimeEndMenu;
        FindMenu();
    }

    private void Update()
    {
        //|| GameManager.Instance.UserDetected()
        if ((Input.GetKeyDown(KeyCode.P) || GameManager.Instance.UserDetected()) && !gameActive) //cambiar esto por detectar al usuario
        {
            Debug.Log("Ha detectado al usuario, empieza el juego");
            //Esto debe ser salir del menu de pausa
            GoToGame();
        }

        //Si lleva mas de 5 segundos sin detectar a nadie y el juego esta corriendo se activa este menu
        if (gameActive && GameManager.Instance.GetTimeWaiting() > maxTimeWaiting && false) //de momento no queremos que entre aqui
        {
            Debug.Log("Lleva mucho tiempo sin encontrar a nadie");
            //esto debe ser entrar al menu de pausa
            FindMenu();
        }

        if (state == MenuState.endScreen)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = maxTimeEndMenu;
                FindMenu();
            }
        }
    }

    public void FindMenu()
    {
        state = MenuState.pauseScreen;
        gameActive = false;
        pauseMenu.SetActive(true);
        endScreen.SetActive(false);
        Time.timeScale = 0;
    }

    public void EndMenu()
    {
        state = MenuState.endScreen;
        gameActive = false;
        endScreen.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void GoToGame()
    {
        state = MenuState.gameScreen;
        gameActive = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        ResetGame();
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
        //SetPause();
        /*
        pauseMenu.SetActive(false);//por si acaso
        Time.timeScale = 1;//por si acaso
        gameActive = true;
        */
    }
}
