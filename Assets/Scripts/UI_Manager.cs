using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text finalScoreTextPointsAndPrices;
    [SerializeField] Text finalScoreTextBase;
    [SerializeField] Text finalMessage;

    public GameObject smallPriceText;
    public GameObject mediumPriceText;
    public GameObject bigPriceText;

    public int price;

    void Start()
    {
        price = 0;
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
        //Has conseguido X puntos y un vale descuento de X dinero.
        if (price == 0)
        {
            finalScoreTextPointsAndPrices.text = $"Has conseguido {GameManager.Instance.score} puntos.";
        }
        else if(price == 1)
        {
            finalScoreTextPointsAndPrices.text = $"Has conseguido {GameManager.Instance.score} puntos y un vale de 5€ para gastar en el centro";
        }
        else if (price == 2)
        {
            finalScoreTextPointsAndPrices.text = $"Has conseguido {GameManager.Instance.score} puntos y un vale de 10€ para gastar en el centro";
        }
        else if (price == 3)
        {
            finalScoreTextPointsAndPrices.text = $"Has conseguido {GameManager.Instance.score} puntos y un vale de 20€ para gastar en el centro";
        }
        finalMessage.text = $"{GameManager.Instance.finalMessage}";
        //por si acaso
        price = 0;
    }
}
