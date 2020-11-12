using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData 
{
    public int prices;
    public int score;
    public bool wonPrice;

    public ScoreData()
    {
        prices = GameManager.Instance.prices;
        score = GameManager.Instance.score;
        wonPrice = GameManager.Instance.wonPrice;
    }
}
