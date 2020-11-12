using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int prices;
    public int maxPresentsScore; //IN
    public int maxPresents; // IN
    public int targetScore; // IN
    public int maxEnergyInScenario; //IN
    public float energyUnitValue;


    public GameData()
    {
        //solo nos interesa actualizar estos parametros, los otros seran solo de entrada
        prices = GameManager.Instance.prices;
        maxPresentsScore = GameManager.Instance.maxPresentsScore;
        maxPresents = GameManager.Instance.maxPresents;
        targetScore = GameManager.Instance.targetScore;
        maxEnergyInScenario = GameManager.Instance.maxEnergyInScenario;
        energyUnitValue = GameManager.Instance.energyUnitValue;
    }

}
