using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Price { none, small, medium, big}

public class PricesManager : MonoBehaviour
{

    /*
     *  Tenemos una determinada cantidad de premios en total
     *  Cada dia se libera uno
     */
    int totalPrices;
    int currentPrices;
    DateTime lastPriceDate;

    //Cargar desde fichero
    int smallPrices; //hay 140 (entre 100 y 150 puntos)
    int mediumPrices; //hay 50 (entre 150 y 200 puntos)
    int bigPrices; //hay 30 (mas de 200 puntos)

    public int leftSmallPrices = 1; //los disponibles de este dia + los que sobraran del dia anterior
    public int leftMediumPrices = 1; //los disponibles de este dia + los que sobraran del dia anterior
    public int leftBigPrices = 1; //los disponibles de este dia + los que sobraran del dia anterior

    public int targetScoreSmall = 100; //TODO: Parametrizar esto
    public int targetScoreMedium = 120;
    public int targetScoreBig = 140;

    //TODO: necesito fechas para saber cuantos premios se repartiran cada dia
    //TODO: puedo hacer un diccionario por date en el cual se especifique cuantos regalos habra cada dia
    //Podemos gestionar todo esto aqui y llamarlo desde el game manager

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

    /*
     Para distribuir los regalos, opciones:
    - Hacer varias curvas y que se repartan por esa curva (la curva se elige aleatoriamente en cada partida)
    - Dejarlos preseteados (asi esta ahora)
     */

    public Price CheckScore(int score)
    {
        //Este metodo lo llamamos cuando acaba la partida
        Price price = Price.none;

        //Caso 1: Si ya no quedan regalos Big se hace que no se pueda conseguir mas de esa puntuacion
        //Caso 2: Si no quedan Small o Medium se suman los puntos que hagan falta para llegar
        if (score >= targetScoreBig)
        {
            if (leftBigPrices > 0)
            {
                //Gana premio grande
                leftBigPrices--;
                price = Price.big;
                GameManager.Instance.finalMessage = "Premio grande";
            }
            else
            {
                //Este caso no debe darse, si no quedan premios grandes no se debe poder llegar aqui
            }
        }
        else if (score >= targetScoreMedium)
        {
            if (leftMediumPrices > 0)
            {
                //Gana premio mediano
                price = Price.medium;
                leftMediumPrices--;
                GameManager.Instance.finalMessage = "Premio mediano";
            }
            else if (leftBigPrices > 0)
            {
                GameManager.Instance.score = score + (targetScoreBig - score); //actualizamos la score
                leftBigPrices--;
                price = Price.big;
                GameManager.Instance.finalMessage = "Premio grande";
            }   
        }
        else if (score >= targetScoreSmall)
        {
            if (leftSmallPrices > 0)
            {
                //Gana premio pequeño
                price = Price.small;
                leftSmallPrices--;
                GameManager.Instance.finalMessage = "Premio pequeño";
            }
            else if (leftMediumPrices > 0) //primero comprobamos si quedan regalos medianos (es el mas cercano al pequeño)
            {
                GameManager.Instance.score = score + (targetScoreMedium - score); //actualizamos la score
                price = Price.medium;
                leftMediumPrices--;
                GameManager.Instance.finalMessage = "Premio mediano";

            }
            else if (leftBigPrices > 0)
            {
                GameManager.Instance.score = score + (targetScoreBig - score); //actualizamos la score
                GameManager.Instance.score = score + (targetScoreBig - score); //actualizamos la score
                leftBigPrices--;
                price = Price.big;
                GameManager.Instance.finalMessage = "Premio grande";
            }


        }
        else
        {
            GameManager.Instance.finalMessage = "Premios conseguidos: ninguno, vete a tu casa";
        }

        UpdateLimit();

        return price;
    }

    public void UpdateLimit()
    {
        if (leftSmallPrices <=0 && leftMediumPrices <= 0 && leftBigPrices <= 0)
        {
            GameManager.Instance.maxPresentsScore = this.targetScoreSmall - 1;
        }
        else if(leftMediumPrices <= 0 && leftBigPrices <= 0)
        {
            GameManager.Instance.maxPresentsScore = this.targetScoreMedium - 1;
        }
        else if (leftBigPrices <= 0)
        {
            GameManager.Instance.maxPresentsScore = this.targetScoreBig - 1;
        }
    }

}
