using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData 
{
    public int prices;
    public int score;
    public string wonPrice;

    public ScoreData()
    {
        prices = GameManager.Instance.prices;
        score = GameManager.Instance.score;
        wonPrice = EnumToString(GameManager.Instance.wonPrice);
    }

    public string EnumToString(Price price)
    {
        if (price == Price.none)
        {
            return "none";
        }
        else if (price == Price.small)
        {
            return "small";
        }
        else if (price == Price.medium)
        {
            return "medium";
        }
        else if (price == Price.big)
        {
            return "big";
        }
        return "none";
    }
}
