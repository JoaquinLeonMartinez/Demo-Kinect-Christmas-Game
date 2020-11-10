using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int score; //regalos que ha recogido el jugador
    public int maxPresentsScore; //numero totoal de regalos que se generan en la escena
    public int maxEnergyInScenario; // no es necesario de momento pero por si se quisiera utilizar
    public int presentSpawnRate;
    public static GameManager Instance;

    //[SerializeField] GameObject[] presentPoints; //lisa de puntos en los que pueden estar los regalos
    [SerializeField] UI_Manager UIManager;
    [SerializeField] EnergyBar energyBar;
    int currentEnergy;
    int maxEnergy;
    float timeElapsed; //Es el tiempo que ha pasado
    float energyUnitValue;

    [SerializeField] List<GameObject> presentPoints;
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
        //ENERGY BAR 
        maxEnergy = 20;
        currentEnergy = maxEnergy;
        timeElapsed = 0;
        energyUnitValue = 2;
        //END ENERGY BAR


        //GAMEPLAY
        //maxEnergyInScenario = 0; //DE MOMENTO NO LO ESTAMOS USANDO
        maxPresentsScore = 0; //empieza habiendo 0
        score = 0;
        presentPoints = new List<GameObject>();
        SetUpPresentsPoints();
        presentSpawnRate = 100; //% de spawn de regalos
        //Generar regalos
        //caso 1: se genera un numero aleatrio con un indice de probabilidad (de este modo en cada partida habra una cantidad diferente de regalos)
        //SetPresents();
        SetPresentsMegaRandom();

        //caso 2: se genera un numero fijo de regalos pero en cada partida estaran en un sitio diferente
        //maxPresents = 40; //EN EL CASO 2 LO INDICAMOS NOSOTROS
        //SetFixNumOfPresentsBetter(maxPresents);
        //ENDGAMEPLAY

        //UI
        energyBar.setMaxEnergy(maxEnergy);
        UpdateScore(score);
        //END UI
    }

    private void Update()
    {
        
        timeElapsed += Time.deltaTime; //con esto sabremos cuanto tiempo ha pasado

        if (timeElapsed >= energyUnitValue)
        {
            timeElapsed = 0;
            currentEnergy--;
            energyBar.setEnergy(currentEnergy);
            CheckEnergy();
        }
    }

    void CheckEnergy()//a este metodo habra que llamarlo cada vez que decrementamos la energía
    {
        if (currentEnergy <= 0)
        {
            Application.Quit(); //en el futuro esto puede ser un volver al menu
        }
    }
    public void SetPresents() //en este modo no siempre habra el mismo numero de regalos
    {
        foreach (var p in presentPoints)
        {
            if (Random.Range(1,100) <= presentSpawnRate)
            {
                p.SetActive(true);
                if (p.GetComponent<Collectible>().collectibleType == CollectibleType.Points)
                {
                    maxPresentsScore += p.GetComponent<Collectible>().value;
                }
            }
        }

        //Debug.Log($"Se han generado {maxPresents} puntos");
    }

    public void SetFixNumOfPresents(int presentsToGenerate) //comprobar eficiencia de esto, podria no ser la mejor
    {
        if (presentsToGenerate > presentPoints.Count) //esto no debe darse nunca, causaria un bucle infinito
        {
            return;
        }
        //int pendingPresents = maxPresents;
        while (presentsToGenerate  > 0)
        {
            int randomPos = Random.Range(0, presentPoints.Count);
            if (!presentPoints[randomPos].activeSelf) //si ese regalo no esta activo ya lo activamos, sino volvemos a intentar
            {
                presentPoints[randomPos].SetActive(true);
                presentsToGenerate--;
                Debug.Log($"Quedan por generar {presentsToGenerate} regalos");
            }
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


    public void SetFixNumOfPresentsBetter(int presentsToGenerate) //version mejorada
    {
        if (presentsToGenerate > presentPoints.Count) //esto no debe darse nunca, causaria un bucle infinito
        {
            return;
        }

        List<GameObject> auxList = new List<GameObject>();

        while (presentsToGenerate > 0)
        {
            int randomPos = Random.Range(0, presentPoints.Count);
            if (!presentPoints[randomPos].activeSelf)
            {
                presentPoints[randomPos].SetActive(true); //lo activamos
                auxList.Add(presentPoints[randomPos]); //add to aux list
                presentPoints.Remove(presentPoints[randomPos]); //lo eliminamos de la lista para la siguiente iteracion
                Debug.Log("Hemos activado un objeto, el tamaño de la lista origal es " + presentPoints.Count);
                presentsToGenerate--;
            }
        }

        for (int i = 0; i < auxList.Count; i++) //volvemos a completar la lista original
        {
            presentPoints.Add(auxList[i]);
        }
        Debug.Log("La lista vuelve a su tamaño original:  " + presentPoints.Count);

        auxList.Clear();

    }

    void SetUpPresentsPoints()
    {
        foreach (Transform child in collectable.transform)
        {
          presentPoints.Add(child.gameObject);
        }
    }

    void SetUpGame()
    {
        /*
         * A partir de una serie de puntos generar X regalos
         * A partir de una serie de puntos generar X puntos
         * A partir de una serie de puntos generar X energia
         */
    }

    public void SetPresentsMegaRandom() //en este modo no siempre habra el mismo numero de regalos
    {
        //esto seria partiendo de una serie de puntos (los puntos serian puntos de verdad)
        foreach (var p in presentPoints)
        {
            if (Random.Range(1, 100) <= presentSpawnRate)
            {
                var newObject = Instantiate(collectiblePrefab, p.transform);
                CollectibleType newType = (Random.Range(1, 3) == 1) ?  CollectibleType.Energy : CollectibleType.Points;

                if (newType == CollectibleType.Points)
                {
                    newObject.GetComponent<Collectible>().Setup(Random.Range(1, 4), CollectibleType.Points);
                    maxPresentsScore += newObject.GetComponent<Collectible>().value;
                }
                else
                {
                    newObject.GetComponent<Collectible>().Setup(Random.Range(1, 4), CollectibleType.Energy);
                }
            }
        }

        //Debug.Log($"Se han generado {maxPresentsScore} puntos");
    }
}
