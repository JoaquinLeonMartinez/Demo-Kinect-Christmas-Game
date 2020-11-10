using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int score; //regalos que ha recogido el jugador
    public int maxPresents; //numero totoal de regalos que se generan en la escena
    public int presentSpawnRate;
    public static GameManager Instance;

    //[SerializeField] GameObject[] presentPoints; //lisa de puntos en los que pueden estar los regalos
    [SerializeField] UI_Manager UIManager;

    [SerializeField] List<GameObject> presentPoints;
    [SerializeField] GameObject collectable;

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
        maxPresents = 0; //empieza habiendo 0
        score = 0;
        //presentSpawnRate = 70; //% de spawn de regalos

        presentPoints = new List<GameObject>();
        SetUpPresentsPoints();

        //Generar regalos
        //caso 1: se genera un numero aleatrio con un indice de probabilidad (de este modo en cada partida habra una cantidad diferente de regalos)
        SetPresents();

        //caso 2: se genera un numero fijo de regalos pero en cada partida estaran en un sitio diferente
        //maxPresents = 40; //EN EL CASO 2 LO INDICAMOS NOSOTROS
        //SetFixNumOfPresentsBetter(maxPresents);



        GameManager.Instance.UpdateScore(score);
        UIManager.UpdateScore(score, maxPresents);
    }

    public void SetPresents() //en este modo no siempre habra el mismo numero de regalos
    {
        foreach (var p in presentPoints)
        {
            if (Random.Range(1,100) <= presentSpawnRate)
            {
                p.SetActive(true);
                maxPresents += p.GetComponent<Collectible>().value;
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

        UIManager.UpdateScore(score, maxPresents);
        //Debug.Log($"Se acaba de actualizar la puntuación a {score}");
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
}
