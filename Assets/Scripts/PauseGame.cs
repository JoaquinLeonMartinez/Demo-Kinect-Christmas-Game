using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState { initScreen, pauseScreen, endScreen, gameScreen};
public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject initMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endScreen;
    public bool gameActive;
    public float maxTimeWaiting;
    public float maxTimeEndMenu;
    float timerEndGame;
    public float maxTimeInitMenu;
    float timerInitMenu;

    public MenuState state;

    private void Start()
    {
        maxTimeWaiting = 5f;
        maxTimeEndMenu = 5f;
        maxTimeInitMenu = 5f;
        timerEndGame = maxTimeEndMenu;
        FindMenu();
        timerInitMenu = maxTimeInitMenu;
    }

    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.P) || GameManager.Instance.UserDetected()) && state == MenuState.initScreen) //pantalla de inicio, si pones los brazos arriba se pasa a pauseScreen
        {
            Debug.Log("Ha detectado al usuario, empieza el juego, pero antes a explicar lo del avioncito");
            GoToPause();
        }

        if (state == MenuState.pauseScreen) //cambiar esto por detectar al usuario
        {
            if (timerInitMenu > 0)
            {
                timerInitMenu -= Time.deltaTime;
            }
            else
            {
                timerInitMenu = maxTimeInitMenu;
                GoToGame();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                timerInitMenu = maxTimeInitMenu;
                GoToGame();
            }
        }

        //Si lleva mas de 5 segundos sin detectar a nadie y el juego esta corriendo se activa este menu
        if (MenuState.gameScreen == state && GameManager.Instance.GetTimeWaiting() > maxTimeWaiting && false) //de momento no queremos que entre aqui
        {
            Debug.Log("Lleva mucho tiempo sin encontrar a nadie");
            //esto debe ser entrar al menu de pausa
            FindMenu();
        }

        if (state == MenuState.endScreen)
        {
            if (timerEndGame > 0)
            {
                timerEndGame -= Time.deltaTime;
            }
            else
            {
                timerEndGame = maxTimeEndMenu;
                FindMenu();
            }
        }
    }

    public void FindMenu()
    {
        state = MenuState.initScreen;
        gameActive = false;
        initMenu.SetActive(true);
        endScreen.SetActive(false);
        pauseMenu.SetActive(false);
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

    public void GoToPause()
    {
        state = MenuState.pauseScreen;
        gameActive = false;
        pauseMenu.SetActive(true);
        initMenu.SetActive(false);
        Time.timeScale = 1;
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
