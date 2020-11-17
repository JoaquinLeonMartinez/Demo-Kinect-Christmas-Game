using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PricesManager : MonoBehaviour
{

    /*
     *  Tenemos una determinada cantidad de premios en total
     *  Cada dia se libera uno
     */
    int totalPrices;
    int currentPrices;
    DateTime lastPriceDate;


    void Start()
    {
        totalPrices = 20; //fichero
        lastPriceDate =  new DateTime(2020, 11, 16); //TODO: esto se debera leer desde un fichero
    }

    void Update()
    {
        //  Opcion 1: (si lo apagan de noche esto no funcionara)
        /*
        if (System.DateTime.Now.Hour == 12 && System.DateTime.Now.Minute == 0 && System.DateTime.Now.Second == 0)
        {
            Debug.Log($"Es la hora!!");
            if(totalPrices > 0)
            {
                totalPrices--;
                currentPrices++;
            }
        }
        */
        //Opcion 2: 
        // Podemos guardar la última fecha en la que se "liberó" algún regalo
        if (lastPriceDate == System.DateTime.Today)
        {
            //hoy ya se ha puesto regalo disponible, asique nada
            Debug.Log($"Hoy ya hemos puesto regalo! - LastPriceDate: {lastPriceDate} - todayDate: {System.DateTime.Today}");
            //Si reinician todos los dias aqui podria haber incluso un destroy
        }
        else if (lastPriceDate < System.DateTime.Today)
        {
            //esto significa que hoy no se han sumado regalos disponibles
            Debug.Log($"Hoy no hemos puesto regalo, habrá que ponerlo! - LastPriceDate: {lastPriceDate} - todayDate: {System.DateTime.Today}");
            if (totalPrices > 0)
            {
                totalPrices--;
                currentPrices++;
                GameManager.Instance.prices = currentPrices;
                lastPriceDate = System.DateTime.Today;
                //TODO: Actualizar la base de datos, tanto el current como el total prices
            }
        }
        
         
    }
}
