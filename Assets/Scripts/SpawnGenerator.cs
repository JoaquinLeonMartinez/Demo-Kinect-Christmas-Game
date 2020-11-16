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

    public int distanceBetweenCollectables;

    public int currentPath = 0; //indica si los regalos se generaran a la derecha centro o izquierda
    public int minSizeGroup = 3;
    public int maxSizeGroup = 6;
    public int currentSize = 0;

    public void GenerateSpawners(int numOfSpawners)
    {
        distanceTravelled = 0;
        float spawnerFrecuency = pathCreator.path.length / (float)numOfSpawners;
        Debug.Log($"path legth:  { pathCreator.path.length }");
        while (distanceTravelled < pathCreator.path.length)
        {
            distanceTravelled += spawnerFrecuency;//(float)(pathCreator.path.length * 0.02);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
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

            transform.SetPositionAndRotation(new Vector3(transform.position.x + distanceBetweenCollectables * currentPath, transform.position.y, transform.position.z), transform.rotation);
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

        Debug.Log($"Se han borrado los spawneres en teoria");
    }


}
