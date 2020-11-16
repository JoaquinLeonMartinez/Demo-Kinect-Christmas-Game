using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType { Energy, Points}

public class Collectible : MonoBehaviour
{
    public float rotateSpeed;
    public int value; //puntos que sumara al ser recogido
    public CollectibleType collectibleType;

    public GameObject scoreSpritePrefab;

    void Start()
    {
        rotateSpeed = 100f;
    }

    public void Setup(int _value = 1, CollectibleType _collectibleType = CollectibleType.Points, float _rotateSpeed = 100f)
    {
        value = _value;
        collectibleType = _collectibleType;
        rotateSpeed = _rotateSpeed;
    }
    
    void Update()
    {

        if (collectibleType == CollectibleType.Points) // solo rotan los regalos, no los aros de energia
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
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
            GameObject childObject = Instantiate(scoreSpritePrefab, this.transform) as GameObject;
            childObject.transform.parent = this.transform.parent;

            GameManager.Instance.UpdateScore(value);
        }

        Debug.Log("Has recogido un objeto :D");

        if (collectibleType == CollectibleType.Energy)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            DestroyCollectible();
        }

        
    }

    public void DestroyCollectible()
    {
        Destroy(gameObject);
    }
}
