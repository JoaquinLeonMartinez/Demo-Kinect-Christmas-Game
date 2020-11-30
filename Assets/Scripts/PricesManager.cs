using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum Price { none, small, medium, big}

public class PricesManager : MonoBehaviour
{

    /*
     *  Tenemos una determinada cantidad de premios en total
     *  Cada dia se libera uno
     */
    public DateTime lastCheck;

    //Cargar desde fichero

    public int leftSmallPrices = 0; //los disponibles de este dia + los que sobraran del dia anterior
    public int leftMediumPrices = 0; //los disponibles de este dia + los que sobraran del dia anterior
    public int leftBigPrices = 0; //los disponibles de este dia + los que sobraran del dia anterior

    int targetScoreSmall = 200; //TODO: Parametrizar esto
    int targetScoreMedium = 220;
    int targetScoreBig = 240;
    int defaultMaxScore = 270;

    //Debug parameters
    [SerializeField] Text small;
    [SerializeField] Text medium;
    [SerializeField] Text big;
    [SerializeField] Text date;

    [SerializeField] UI_Manager ui_manager;

    Dictionary<DateTime, DayPrices> pricesPerDay = new Dictionary<DateTime, DayPrices>();

    void Start()
    {
        //19 dias-- Repartir:
        //hay 140 (entre 100 y 150 puntos) (7 DIAS DEBE HABER 8 REGALOS Y EL RESTO 7 REGALOS)
        //hay 50 (entre 150 y 200 puntos) (12 DIAS DEBE HABER 3 REGALOS Y EL RESTO 2 REGALOS)
        //hay 30 (mas de 200 puntos) (11 DIAS DEBE HABER 2 REGALOS Y EL RESTO 1 REGALOS)
        pricesPerDay.Add(new DateTime(2020, 11, 19), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 11, 20), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 11, 23), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 11, 24), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 11, 25), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 11, 26), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 11, 27), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 11, 30), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 01), new DayPrices(5, 3, 2));
        pricesPerDay.Add(new DateTime(2020, 12, 02), new DayPrices(5, 3, 2));

        //A partir de aqui son los reales
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

        LoadLastCheck(); //Cargamos de fichero la fecha del ultimo check que se hizo
        //lastCheck = DateTime.Today;

        LoadPricesData(); //Cargamos los premios que sobraron

        Debug.Log($"lastCheck = {lastCheck}, prices: small = {leftSmallPrices}, medium = {leftMediumPrices}, big = {leftBigPrices}");
    }


    void Update()
    {
        CheckTodaysPrices(); //tenemos que comprobar que no pasemos de dia (podriamos hacer un timer de 24 horas, pero es tonteria ya quee sto solo tiene una comparacion)

        small.text = "SmallPricesLeft:" + leftSmallPrices;
        medium.text = "MediumPricesLeft:" + leftMediumPrices;
        big.text = "BigPricesLeft:" + leftBigPrices;
        date.text = "LastCheck:" + lastCheck;
    }



    public void CheckTodaysPrices()
    {
        //1 - Consultar que dia es
        //2 - Copnsultar en el diccionario los regalos correspondientes y sumarlo
        //3 - esto no hace falta que este en un update realmente

        // 0 si son iguales, si el primero es anterior al segundo es menor de cero
        if (DateTime.Compare(lastCheck, DateTime.Today) < 0) //si lastCheck es menor que today entramos
        {
            //Actualizamos:
            DayPrices todayPrices = null; //por defecto habra 0 regalos
            pricesPerDay.TryGetValue(System.DateTime.Today, out todayPrices);
            if (todayPrices == null)
            {
                todayPrices = new DayPrices(0, 0, 0);
            }
            leftSmallPrices += todayPrices.smallPrices;
            leftMediumPrices += todayPrices.mediumPrices;
            leftBigPrices += todayPrices.bigPrices;
            lastCheck = System.DateTime.Today;
            SaveLastCheck();
            Debug.Log("Actualizamos al inicio porque es un nuevo dia");
            SaveDataPrices(); //por si se apagara el juego antes de finalizar la primera partida, sin esta linea se perderian los premios de este dia en ese caso
            UpdateLimit(); //por si ha cambiado
            //Debug.Log("Hemos cargado la informacion de ayer");
        }
        else
        {
            //Hoy ya lo has chequeado
            //Debug.Log("Hoy no hay nah pa ti!");
        }
    }

    public void LoadPricesData()
    {
        leftSmallPrices = PlayerPrefs.GetInt("leftSmallPrices");
        leftMediumPrices = PlayerPrefs.GetInt("leftMediumPrices");
        leftBigPrices = PlayerPrefs.GetInt("leftBigPrices");
    }

    public void LoadLastCheck()
    {
        //public DateTime (int year, int month, int day);
        lastCheck = new DateTime(PlayerPrefs.GetInt("lastCheckYear"), PlayerPrefs.GetInt("lastCheckMonth"), PlayerPrefs.GetInt("lastCheckDay"));
    }

    public void SaveDataPrices()
    {
        PlayerPrefs.SetInt("leftSmallPrices", leftSmallPrices);
        PlayerPrefs.SetInt("leftMediumPrices", leftMediumPrices);
        PlayerPrefs.SetInt("leftBigPrices", leftBigPrices);
        PlayerPrefs.Save();
    }

    public void SaveLastCheck()
    {
        //LastCheck = 19/11/2020 0:00:00
        PlayerPrefs.SetInt("lastCheckDay", lastCheck.Day);
        PlayerPrefs.SetInt("lastCheckMonth", lastCheck.Month);
        PlayerPrefs.SetInt("lastCheckYear", lastCheck.Year);
        PlayerPrefs.Save();
    }

    public void disablePrices()
    {
        ui_manager.disablePrices();
    }

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
                ui_manager.bigPriceText.SetActive(true);
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
                ui_manager.mediumPriceText.SetActive(true);
            }
            else if (leftBigPrices > 0)
            {
                GameManager.Instance.score = score + (targetScoreBig - score); //actualizamos la score
                leftBigPrices--;
                price = Price.big;
                GameManager.Instance.finalMessage = "Premio grande";
                ui_manager.bigPriceText.SetActive(true);
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
                ui_manager.smallPriceText.SetActive(true);
            }
            else if (leftMediumPrices > 0) //primero comprobamos si quedan regalos medianos (es el mas cercano al pequeño)
            {
                GameManager.Instance.score = score + (targetScoreMedium - score); //actualizamos la score
                price = Price.medium;
                leftMediumPrices--;
                GameManager.Instance.finalMessage = "Premio mediano";
                ui_manager.mediumPriceText.SetActive(true);

            }
            else if (leftBigPrices > 0)
            {
                GameManager.Instance.score = score + (targetScoreBig - score); //actualizamos la score
                GameManager.Instance.score = score + (targetScoreBig - score); //actualizamos la score
                leftBigPrices--;
                price = Price.big;
                GameManager.Instance.finalMessage = "Premio grande";
                ui_manager.bigPriceText.SetActive(true);
            }
        }
        else
        {
            GameManager.Instance.finalMessage = "Premios conseguidos: ninguno, vete a tu casa";
        }

        //Entre partida y partida esto ya lo esta llamando el propio manager, por lo tanto n oes necesario llamarlo aqui
        UpdateLimit();
        SaveDataPrices();//actualizamos la base de datos

        return price;
    }

    public void UpdateLimit()
    {
        //Debug.Log($"targetScoreSmall: {targetScoreSmall} - targetScoreMedium: {targetScoreMedium} - maxPresentsScore: {targetScoreBig} ");
        if (leftSmallPrices <= 0 && leftMediumPrices <= 0 && leftBigPrices <= 0)
        {
            GameManager.Instance.maxPresentsScore = this.targetScoreSmall - 1;
        }
        else if (leftMediumPrices <= 0 && leftBigPrices <= 0)
        {
            GameManager.Instance.maxPresentsScore = this.targetScoreMedium - 1;
        }
        else if (leftBigPrices <= 0)
        {
            GameManager.Instance.maxPresentsScore = this.targetScoreBig - 1;
        }
        else
        {
            GameManager.Instance.maxPresentsScore = this.defaultMaxScore;
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
