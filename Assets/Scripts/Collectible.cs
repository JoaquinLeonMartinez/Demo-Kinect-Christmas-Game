using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public int value; //puntos que sumara al ser recogido
    // Start is called before the first frame update
    void Start()
    {
        //value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 2, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.UpdateScore(value);
        Debug.Log("Has recogido un regalo :D");
        Destroy(gameObject);
    }
}
