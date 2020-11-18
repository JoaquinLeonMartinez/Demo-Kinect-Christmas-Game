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
    DateTime lastCheck;

    //Cargar desde fichero

    public int leftSmallPrices = 1; //los disponibles de este dia + los que sobraran del dia anterior
    public int leftMediumPrices = 1; //los disponibles de este dia + los que sobraran del dia anterior
    public int leftBigPrices = 1; //los disponibles de este dia + los que sobraran del dia anterior

    public int targetScoreSmall = 200; //TODO: Parametrizar esto
    public int targetScoreMedium = 220;
    public int targetScoreBig = 240;

    //TODO: necesito fechas para saber cuantos premios se repartiran cada dia
    //TODO: puedo hacer un diccionario por date en el cual se especifique cuantos regalos habra cada dia
    //Podemos gestionar todo esto aqui y llamarlo desde el game manager

    Dictionary<DateTime, DayPrices> pricesPerDay = new Dictionary<DateTime, DayPrices>();

    void Start()
    {
        //19 dias-- Repartir:
        //hay 140 (entre 100 y 150 puntos) (7 DIAS DEBE HABER 8 REGALOS Y EL RESTO 7 REGALOS)
        //hay 50 (entre 150 y 200 puntos) (12 DIAS DEBE HABER 3 REGALOS Y EL RESTO 2 REGALOS)
        //hay 30 (mas de 200 puntos) (11 DIAS DEBE HABER 2 REGALOS Y EL RESTO 1 REGALOS)
        pricesPerDay.Add(new DateTime(2020, 12, 11), new DayPrices(8, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 12), new DayPrices(8, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 13), new DayPrices(8, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 14), new DayPrices(8, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 15), new DayPrices(8, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 16), new DayPrices(8, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 17), new DayPrices(8, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 18), new DayPrices(7, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 19), new DayPrices(7, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 20), new DayPrices(7, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 21), new DayPrices(7, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 22), new DayPrices(7, 3, 1));
        pricesPerDay.Add(new DateTime(2020, 12, 23), new DayPrices(7, 2, 1));
        pricesPerDay.Add(new DateTime(2020, 12, 24), new DayPrices(7, 2, 1));
        pricesPerDay.Add(new DateTime(2020, 12, 25), new DayPrices(7, 2, 1));
        pricesPerDay.Add(new DateTime(2020, 12, 26), new DayPrices(7, 2, 1));
        pricesPerDay.Add(new DateTime(2020, 12, 27), new DayPrices(7, 2, 1));
        pricesPerDay.Add(new DateTime(2020, 12, 28), new DayPrices(7, 2, 1));
        pricesPerDay.Add(new DateTime(2020, 12, 29), new DayPrices(7, 2, 1));

        lastCheck = System.DateTime.Today;//Esta linea es temporal, esto debewra leerse desde un fichero
        CheckTodaysPrices();
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
        /*
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
        */
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

    public void CheckTodaysPrices()
    {
        //1 - Consultar que dia es
        //2 - Copnsultar en el diccionario los regalos correspondientes y sumarlo
        //3 - esto no hace falta que este en un update realmente

        if (lastCheck <= System.DateTime.Today && false)
        {
            DayPrices todayPrices = new DayPrices(0, 0, 0);
            pricesPerDay.TryGetValue(System.DateTime.Today, out todayPrices);
            leftSmallPrices += todayPrices.smallPrices;
            leftMediumPrices += todayPrices.mediumPrices;
            leftBigPrices += todayPrices.bigPrices;
            lastCheck = System.DateTime.Today;
        }
        else
        {
            //Hoy ya lo has chequeado
        }

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

public class DayPrices
{
    DateTime dateTime;
    public int smallPrices;
    public int mediumPrices;
    public int bigPrices;

    public DayPrices(int _smallPrices, int _mediumPrices, int _bigPrices)
    {
        smallPrices = _smallPrices;
        mediumPrices = _mediumPrices;
        bigPrices = _bigPrices;
    }
}
