using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] Slider slider;


    public void setEnergy(int energy) //actualiza la barra
    {
        if (energy > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
        else
        {
            slider.value = energy;
        } 
    }

    public void setMaxEnergy(int energy) //se llama al inicio para establecer el limite de energia
    {
        slider.maxValue = energy;
        slider.value = slider.maxValue;
    }
}
