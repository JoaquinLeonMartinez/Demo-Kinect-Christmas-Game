using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    void Start()
    {
        //scoreText.text = "Score: 0 / 0"; // no hace falta ya que el propio game manager es el encargado de actualizarlo al inicio
    }

    void Update()
    {
        
    }

    public void UpdateScore(int currentScore, int maxScore)
    {
        //scoreText.text = $"Score: {currentScore} / {maxScore}";
        scoreText.text = $"{currentScore}";
    }
}
