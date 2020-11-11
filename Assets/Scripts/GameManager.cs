﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int score; //regalos que ha recogido el jugador
    public int maxPresentsScore; //numero totoal de regalos que se generan en la escena
    public int maxPresents;
    public int targetScore;
    public bool prices;
    public int smallPresents;
    public int mediumPresents;
    public int bigPresents;
    public int maxEnergyInScenario;

    public static GameManager Instance;

    [SerializeField] GameObject playerController;
    [SerializeField] UI_Manager UIManager;
    [SerializeField] EnergyBar energyBar;
    int currentEnergy;
    int maxEnergy;
    float timeElapsed; //Es el tiempo que ha pasado
    float energyUnitValue;

    [SerializeField] List<GameObject> collectiblePoints;
    [SerializeField] GameObject collectable;

    [SerializeField] GameObject collectiblePrefab;

    void Awake()
    {

        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            //Rest of your Awake code

        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        collectiblePoints = new List<GameObject>();
        SetupPresentsPoints();
        SetUpGame();
    }

    private void Update()
    {

        DebugFuntion(); //borrar antes de subir

        timeElapsed += Time.deltaTime; //con esto sabremos cuanto tiempo ha pasado

        if (timeElapsed >= energyUnitValue)
        {
            timeElapsed = 0;
            currentEnergy--;
            energyBar.setEnergy(currentEnergy);
            CheckEnergy();
        }
    }

    void DebugFuntion()
    {
        //Debug.Log($"Numero total de regalos: {maxPresents}");
        //Debug.Log($"Numero total de regalos: {maxPresents}");
    }

    void CheckEnergy()//a este metodo habra que llamarlo cada vez que decrementamos la energía
    {
        if (currentEnergy <= 0)
        {
            CheckScore();
            this.GetComponent<PauseGame>().SetPause();
        }
    }

    void CheckScore()//comprobamos si ha conseguido el premio
    {
        if(score >= targetScore)
        {
            Debug.Log("Enhorabuena, te llevas un fantastico masajeador de pies!!");
            //TODO: Aqui habra que comprobar si quedan premios o no y actuar en consecuencia
        }
    }

    public void UpdateScore(int _score)
    {
        this.score += _score;

        if (score < 0) //en caso de haber algo que reste puntos
        {
            score = 0;
        }

        UIManager.UpdateScore(score, maxPresentsScore);
        //Debug.Log($"Se acaba de actualizar la puntuación a {score}");
    }

    public void UpdateEnergy(int _energy)
    {
        this.currentEnergy += _energy;
        energyBar.setEnergy(currentEnergy);
    }


    void SetupPresentsPoints()
    {
        foreach (Transform child in collectable.transform)
        {
            collectiblePoints.Add(child.gameObject);
            //comprobar si estos tienen hijos, si los tienen hay que destruirlos
        }
    }

    public void ResetGame()
    {
        playerController.GetComponent<Follower>().ResetFollower();
        playerController.GetComponent<Controll>().ResetControll();
        ResetCollectables();
        SetUpGameReset();
    }

    public void SetUpGame()
    {
        //ENERGY BAR 
        maxEnergy = 20;
        currentEnergy = maxEnergy;
        timeElapsed = 0;
        energyUnitValue = 2;
        //END ENERGY BAR


        //GAMEPLAY
        prices = true; //si quedan premios o no en el centro comercial (add que cada hora se actualice)
        maxEnergyInScenario = 5; //aros de energía que encontraremos
        maxPresents = 15; //numero de regalos en la escena
        maxPresentsScore = 30; //numero maximo de puntos que podremos sumar (deben de ser mayor que el maxPresents)
        score = 0; //puntuacion actual
        targetScore = 20; //puntuacion objetivo
        //La propia funcion de generar calcula cuantos habra de cada tipo (distribuye los puntos en el numero de regalos)
        smallPresents = 0;
        mediumPresents = 0;
        bigPresents = 0;

        if (CheckGameRules()) //comprobamos que los valores sean coherentes
        {
            Debug.Log("Los parametros son correctos, vamos a generar los collectible");
            GenerateGameCollectables();
        }
        else
        {
            Debug.Log("Los parametros no son correctos hulio");
        }
 
        //ENDGAMEPLAY

        //UI
        energyBar.setMaxEnergy(maxEnergy);
        UpdateScore(score);
        //END UI
    }

    public void SetUpGameReset()
    {
        //ENERGY BAR 
        maxEnergy = 20;
        currentEnergy = maxEnergy;
        timeElapsed = 0;
        energyUnitValue = 2;
        //END ENERGY BAR


        //GAMEPLAY
        prices = true; //si quedan premios o no en el centro comercial (add que cada hora se actualice)
        maxEnergyInScenario = 5; //aros de energía que encontraremos
        maxPresents = 15; //numero de regalos en la escena
        maxPresentsScore = 30; //numero maximo de puntos que podremos sumar (deben de ser mayor que el maxPresents)
        score = 0; //puntuacion actual
        targetScore = 20; //puntuacion objetivo
        //La propia funcion de generar calcula cuantos habra de cada tipo (distribuye los puntos en el numero de regalos)
        smallPresents = 0;
        mediumPresents = 0;
        bigPresents = 0;

        Debug.Log("Vamos a comprobar si todo es correcto:");
        
        if (CheckGameRules()) //comprobamos que los valores sean coherentes
        {
            Debug.Log("Los parametros son correctos, vamos a generar los collectible");
            GenerateGameCollectables();
        }
        else
        {
            Debug.Log("Los parametros no son correctos hulio");
        }
        
        //ENDGAMEPLAY

        //UI
        energyBar.setMaxEnergy(maxEnergy);
        UpdateScore(score);
        //END UI
    }

    public void GenerateGameCollectables()
    {
        DivideRatesFunction(maxPresentsScore, maxPresents);
        Generate(maxPresents, maxEnergyInScenario);
    }
    public void DivideRatesFunction(int pointsToDistribute, int numOfPresents)
    {

        if (!prices) //si no quedan premios el limite sera menor que el target
        {
            pointsToDistribute = targetScore - 1;
        }

        int i = 0;
        int maxRandom = 4;
        while (i < numOfPresents && (pointsToDistribute > (numOfPresents - (mediumPresents + bigPresents))))
        {
            bool validRandom = false;
            int random = Random.Range(2, maxRandom);
            if (random == 2 && (pointsToDistribute > (numOfPresents - (mediumPresents + bigPresents))))
            {
                pointsToDistribute -= 2;
                mediumPresents++;
                validRandom = true;
            }
            else if (random == 3 && (pointsToDistribute > ((numOfPresents - (mediumPresents + bigPresents)) + 1)))
            {
                pointsToDistribute -= 3;
                bigPresents++;
                validRandom = true;
            }
            else if (random == 3 && !(pointsToDistribute > ((numOfPresents - (mediumPresents + bigPresents)) + 1)))
            {
                maxRandom = 3; //descartamos los regalos de 3 puntos
            }

            if (validRandom)
            {
                i++;
            }
        }
        //Debug.Log($"Suma final de los que restan de un punto: smallPresents: {smallPresents} + points to distribute restantes: {pointsToDistribute}");
        smallPresents += pointsToDistribute; //esto sumara 0 en caso de haberlos distribuido todos antes
        Debug.Log($"Se han distribuido correctamente los valores de los regalos, small = {smallPresents} , medium = {mediumPresents} , big = {bigPresents}");

    }

    public void Generate(int presentsToGenerate, int eneryToGenerate)
    {
        List<GameObject> auxList = new List<GameObject>();

        //Ahora vamos con los aros de energia:

        while (eneryToGenerate > 0)
        {
            int randomPos = Random.Range(0, collectiblePoints.Count);
                //instanciate
            var newObject = Instantiate(collectiblePrefab, collectiblePoints[randomPos].transform);
            newObject.GetComponent<Collectible>().Setup(1, CollectibleType.Energy);

            auxList.Add(collectiblePoints[randomPos]); //add to aux list
            collectiblePoints.Remove(collectiblePoints[randomPos]); //lo eliminamos de la lista para la siguiente iteracion

            eneryToGenerate--; //esto dependera del random que se haya generado
        }

        Debug.Log("Se han generado correctamente los aros de energia");

        //Ahora vamos con los regalos:

        while (presentsToGenerate > 0)
        {
            int randomPos = Random.Range(0, collectiblePoints.Count);
            int newValue = 0;
            if (bigPresents > 0)
            {
                newValue = 3;
                bigPresents--;
            }
            else if (mediumPresents > 0)
            {
                newValue = 2;
                mediumPresents--;
            }
            else
            {
                newValue = 1;
                smallPresents--;//esto realmente no es necesario
            }

            var newObject = Instantiate(collectiblePrefab, collectiblePoints[randomPos].transform);
            newObject.GetComponent<Collectible>().Setup(newValue, CollectibleType.Points);

            auxList.Add(collectiblePoints[randomPos]); //add to aux list
            collectiblePoints.Remove(collectiblePoints[randomPos]); //lo eliminamos de la lista para la siguiente iteracion

            presentsToGenerate--; //esto dependera del random que se haya generado

        }

        Debug.Log("Se han generado correctamente los regalos");

        for (int i = 0; i < auxList.Count; i++) //volvemos a completar la lista original
        {
            collectiblePoints.Add(auxList[i]);
        }
        Debug.Log("La lista vuelve a su tamaño original:  " + collectiblePoints.Count);

        auxList.Clear();
    }

    public bool CheckGameRules() //validaciones
    {
        bool correct = true;

        if (!(maxEnergyInScenario + maxPresents <= collectiblePoints.Count))
        {
            Debug.Log($"No cuadra la energia con el nuemro de slots disponible en el escenario, energia = {maxEnergyInScenario} , maxPresents = {maxPresents} , slots = {collectiblePoints.Count}");
            correct = false;
            return correct;
        }

        if (targetScore > maxPresentsScore)
        {
            Debug.Log($"No cuadra la target score, targetScore = {targetScore} , maxPresentsScore = {maxPresentsScore}");
            correct = false;
            return correct;
        }

        return correct;
    }

    void ResetCollectables()
    {
        foreach (var p in collectiblePoints)
        {
            foreach (Transform child in p.transform)
            {
                child.GetComponent<Collectible>().DestroyCollectible();
            }
        }
    }

}

