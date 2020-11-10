using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType { Energy, Points}

public class Collectible : MonoBehaviour
{
    public float rotateSpeed;
    public int value; //puntos que sumara al ser recogido
    public CollectibleType collectibleType;

    void Start()
    {
        rotateSpeed = 2f;
    }

    public void Setup(int _value = 1, CollectibleType _collectibleType = CollectibleType.Points, float _rotateSpeed = 2f)
    {
        value = _value;
        collectibleType = _collectibleType;
        rotateSpeed = _rotateSpeed;
    }
    
    void Update()
    {

        if (collectibleType == CollectibleType.Points) // solo rotan los regalos, no los aros de energia
        {
            transform.Rotate(0, 1*rotateSpeed, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collectibleType == CollectibleType.Energy)
        {
            GameManager.Instance.UpdateEnergy(value);
        }
        else
        {
            GameManager.Instance.UpdateScore(value);
        }
        
        Debug.Log("Has recogido un objeto :D");
        Destroy(gameObject);
    }
}
