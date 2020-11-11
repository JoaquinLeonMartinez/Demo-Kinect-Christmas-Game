using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSprite : MonoBehaviour
{
    public float verticalSpeed;
    public float transparencySpeed;
    float opacity;

    void Start()
    {
        opacity = 1f;
    }


    void Update()
    {
        if (opacity > 0f)
        {
            opacity -= Time.deltaTime * transparencySpeed;
            this.transform.localPosition += Time.deltaTime * verticalSpeed * new Vector3(0, 1, 0);
            this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, opacity);
        }
        else
        {
            DestroyScoreSprite();
        }
    }

    public void DestroyScoreSprite()
    {
        Destroy(gameObject);
    }
}
