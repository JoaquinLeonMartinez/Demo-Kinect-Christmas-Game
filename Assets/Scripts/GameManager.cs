using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int score; // OUT (En caso de querer guardar estadisticas)
    public string finalMessage = "Premios conseguidos: ninguno, vete a tu casa";
    public int maxPresentsScore; //IN
    public int maxPresents; // IN
    public int targetScore; // IN
    public int prices; // IN
    public Price wonPrice; 
    public int smallPresents;
    public int mediumPresents;
    public int bigPresents;
    public int maxEnergyInScenario; //IN
    public bool playing;
    //public int numOfSpawners; //parametrizar esto

    public static GameManager Instance;

    [SerializeField] GameObject playerController;
    [SerializeField] UI_Manager UIManager;
    [SerializeField] EnergyBar energyBar;
    int currentEnergy;
    int maxEnergy;
    float timeElapsed; //Es el tiempo que ha pasado
    public float energyUnitValue;

    [SerializeField] List<GameObject> collectiblePoints;
    [SerializeField] GameObject collectable;

    [SerializeField] GameObject collectiblePrefab;
    [SerializeField] List<GameObject> collectiblePrefabs;
    [SerializeField] GameObject ringsParent;

    //[SerializeField] SpawnGenerator spawnGenerator;

    [SerializeField] BodyReader bodyReader;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            //De poner mas codigo deberia ser aqui
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        /*
        spawnGenerator.GenerateSpawners(numOfSpawners); //generamos los puntos de spawn
        collectiblePoints = new List<GameObject>();
        SetupPresentsPoints(); // los guardamos en una lista
        */
        collectiblePoints = new List<GameObject>();
        //ENERGY BAR 
        maxEnergy = 20;
        currentEnergy = maxEnergy;
        timeElapsed = 0;
        playing = false;
        //END ENERGY BAR

        //GAMEPLAY
        LoadData(); //se cargan desde el json los parametros del juego


        //UI
        energyBar.setMaxEnergy(maxEnergy);
        UpdateScore(score);
        //END UI
    }

    private void Update()
    {
        if (playing)
        {
            if (timeElapsed >= energyUnitValue)
            {
                timeElapsed = 0;
                currentEnergy--;
                energyBar.setEnergy(currentEnergy);
                CheckEnergy();
            }
            else
            {
                timeElapsed += Time.deltaTime; //con esto sabremos cuanto tiempo ha pasado
            }
        }
    }

    void CheckEnergy()//a este metodo habra que llamarlo cada vez que decrementamos la energía
    {
        if (currentEnergy <= 0)
        {
            playing = false;
            wonPrice = this.GetComponent<PricesManager>().CheckScore(score); //esto se encarga de actualizar los puntos en caso de que sea necesario
            UIManager.UpdateScore(score, maxPresentsScore); //esto ira fuera
            UIManager.UpdateFinalScreen();
            this.GetComponent<PauseGame>().EndMenu();
        }
    }

    public void UpdateScore(int _score)
    {
        if (this.GetComponent<PauseGame>().state == MenuState.gameScreen)
        {
            this.score += _score;
            UIManager.UpdateScore(score, maxPresentsScore); //esto ira fuera
        }
    }

    public void UpdateEnergy(int _energy)
    {
        if (_energy > maxEnergy)
        {
            this.currentEnergy = maxEnergy;
        }
        else
        {
            this.currentEnergy += _energy;
        }
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
        playerController.GetComponent<Controll>().ResetControll();
        //ResetCollectables();
        SetUpGame();
    }

    public void SetUpGame()
    {
        ResetRings();
        ResetCollectables(); //Borramos los regalos de los spawners
        //spawnGenerator.DestroySpawners(); //Destruimos los spawners
        //Debug.Log($"Vamos a generar los spawners, en total generaremos {numOfSpawners}");
        //spawnGenerator.GenerateSpawners(numOfSpawners); //Generamos los nuevos puntos de spawn
        SetupPresentsPoints(); // Los guardamos en una lista


        //Reboot values
        currentEnergy = maxEnergy;
        timeElapsed = 0;
        score = 0; 
        smallPresents = 0;
        mediumPresents = 0;
        bigPresents = 0;
        wonPrice = Price.none;
        playing = true;

        Debug.Log($"maxPresentsScoreActual = {maxPresentsScore} - LeftBig = {this.GetComponent<PricesManager>().leftBigPrices} - LeftMedium = {this.GetComponent<PricesManager>().leftMediumPrices} - LeftSmall = {this.GetComponent<PricesManager>().leftSmallPrices}");
        if (CheckGameRules()) //comprobamos que los valores sean coherentes
        {
            //Debug.Log("Los parametros son correctos, vamos a generar los collectible");
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

    public float GetTimeWaiting()
    {
        return bodyReader.timeWaiting;
    }

    public bool UserDetected()
    {
        return bodyReader.userDetected;
    }

    public void GenerateGameCollectables()
    {
        DivideRatesFunction(maxPresentsScore, maxPresents);
        Generate(maxPresents, maxEnergyInScenario);
    }
    public void DivideRatesFunction(int pointsToDistribute, int numOfPresents)
    {

        if (prices <= 0) //si no quedan premios el limite sera menor que el target
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
                pointsToDistribute -= (2*2);
                mediumPresents++;
                validRandom = true;
            }
            else if (random == 3 && (pointsToDistribute > ((numOfPresents - (mediumPresents + bigPresents)) + 1)))
            {
                pointsToDistribute -= (3*2);
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
        //TODO: de momento vamos a comentar los aros:
        /*
        while (eneryToGenerate > 0)
        {
            int randomPos = Random.Range(0, collectiblePoints.Count);
                //instanciate
            var newObject = Instantiate(collectiblePrefabs[3], collectiblePoints[randomPos].transform);
            //newObject.GetComponent<Collectible>().Setup(1, CollectibleType.Energy); // no es necesario, ya le estamos indicando que es el prefab 4, que es el anillo 

            auxList.Add(collectiblePoints[randomPos]); //add to aux list
            collectiblePoints.Remove(collectiblePoints[randomPos]); //lo eliminamos de la lista para la siguiente iteracion

            eneryToGenerate--; //esto dependera del random que se haya generado
        }

        Debug.Log("Se han generado correctamente los aros de energia");
        */


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

            var newObject = Instantiate(collectiblePrefabs[newValue-1], collectiblePoints[randomPos].transform);
            //newObject.GetComponent<Collectible>().Setup(newValue, CollectibleType.Points); //en el propio prefab se indica esta info

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
        Debug.Log($"Numero de spawners: {collectiblePoints.Count}");
        if (!(maxEnergyInScenario + maxPresents <= collectiblePoints.Count))
        {
            Debug.Log($"No cuadra la energia con el nuemro de slots disponible en el escenario, energia = {maxEnergyInScenario} , maxPresents = {maxPresents} , slots = {collectiblePoints.Count}");
            correct = false;
            return correct;
        }
        

        return correct;
    }

    void ResetCollectables()
    {
        collectiblePoints.Clear();

        Debug.Log($"Se han borrado, Spawners: {collectiblePoints.Count}");
    }

    public void ResetRings()
    {
        foreach (Transform ring in ringsParent.transform)
        {
            ring.gameObject.SetActive(true);
        }
    }

    public void SaveData()
    {
        SaveSystem.UpdateGameData();
    }

    public void LoadData()
    {
        //TODO: Si esto devuelve null deberiamos setear unos valores por defecto
        GameData data = SaveSystem.LoadData();

        prices = data.prices;
        maxPresentsScore = data.maxPresentsScore;
        maxPresents = data.maxPresents;
        targetScore = data.targetScore;
        maxEnergyInScenario = data.maxEnergyInScenario;
        energyUnitValue = data.energyUnitValue;

        Debug.Log($"Se han cargado los siguientes datos: score: {score}, maxPresentsScore = {maxPresentsScore}, maxPresents = {maxPresents}, targetScore = {targetScore}, maxEnergyInScenario = {maxEnergyInScenario}");
    }
}

