using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SpawnGenerator : MonoBehaviour
{

    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    float distanceTravelled;

    public GameObject spawnerParent;
    public GameObject spawnerPrefab;

    public int distanceBetweenCollectables = 15;

    public int currentPath = 0; //indica si los regalos se generaran a la derecha centro o izquierda
    public int minSizeGroup = 3;
    public int maxSizeGroup = 6;
    public int currentSize = 0;

    public void GenerateSpawners(int numOfSpawners)
    {
        distanceBetweenCollectables = 5; 
        distanceTravelled = 0;
        float spawnerFrecuency = pathCreator.path.length / (float)numOfSpawners;
        //Debug.Log($"path legth:  { pathCreator.path.length }");
        while (distanceTravelled < pathCreator.path.length)
        {
            distanceTravelled += spawnerFrecuency;
            transform.SetPositionAndRotation(pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction), pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction));

            if (currentSize <= 0)
            {
                currentSize = Random.Range(minSizeGroup, maxSizeGroup + 1);
                int nextPath = Random.Range(0, 3);
                while (currentPath == nextPath)
                {
                    nextPath = Random.Range(0, 3);
                }
                currentPath = nextPath;
            }

            transform.localPosition = new Vector3(transform.position.x + distanceBetweenCollectables * transform.position.normalized.x * currentPath, transform.position.y, transform.position.z + distanceBetweenCollectables * transform.position.normalized.z * currentPath);
            var spawner = Instantiate(spawnerPrefab, transform);
            spawner.transform.parent = spawnerParent.transform; //todos los spawners son hijos del mismo objeto
            currentSize--;
        }
    }

    public void DestroySpawners()
    {
        foreach (Transform child in spawnerParent.transform)
        {
            Destroy(child.gameObject); //esto es lo que no tira
        }

        Debug.Log($"Se han borrado los spawners hijos del objeto parent");
    }


}
