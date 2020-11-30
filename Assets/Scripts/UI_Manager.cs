using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text finalScoreText;
    [SerializeField] Text finalMessage;

    public GameObject smallPriceText;
    public GameObject mediumPriceText;
    public GameObject bigPriceText;

    void Start()
    {
        //scoreText.text = "Score: 0 / 0"; // no hace falta ya que el propio game manager es el encargado de actualizarlo al inicio
    }

    void Update()
    {
        
    }

    public void disablePrices()
    {
        smallPriceText.SetActive(false);
        mediumPriceText.SetActive(false);
        bigPriceText.SetActive(false);
    }

    public void UpdateScore(int currentScore, int maxScore)
    {
        //scoreText.text = $"Score: {currentScore} / {maxScore}";
        scoreText.text = $"{currentScore}";
    }

    public void UpdateFinalScreen()
    {
        finalScoreText.text = $"Puntos conseguidos: {GameManager.Instance.score}";
        finalMessage.text = $"{GameManager.Instance.finalMessage}";
    }
}
