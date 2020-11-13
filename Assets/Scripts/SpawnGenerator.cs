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

    public void GenerateSpawners()
    {

        while (distanceTravelled < pathCreator.path.length)
        {
            distanceTravelled += (float)(pathCreator.path.length * 0.05);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction); 
            var spawner = Instantiate(spawnerPrefab, transform);
            spawner.transform.parent = spawnerParent.transform; //tdos los spawners son hijos del mismo objeto

            //Derecha:
            transform.SetPositionAndRotation(new Vector3(transform.position.x + distanceBetweenCollectables, transform.position.y, transform.position.z), transform.rotation);
            var spawner2 = Instantiate(spawnerPrefab, transform);
            spawner2.transform.parent = spawnerParent.transform; //tdos los spawners son hijos del mismo objeto

            //Izquierda:
            transform.SetPositionAndRotation(new Vector3(transform.position.x - 2 * distanceBetweenCollectables, transform.position.y, transform.position.z), transform.rotation);
            var spawner3 = Instantiate(spawnerPrefab, transform);
            spawner3.transform.parent = spawnerParent.transform; //tdos los spawners son hijos del mismo objeto

             Debug.Log("Generamos un spawner en la posicion ");
        }

    }


}
