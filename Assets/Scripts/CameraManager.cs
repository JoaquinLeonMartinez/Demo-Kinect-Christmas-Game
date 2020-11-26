using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera gameCam;
    [SerializeField] Camera menuCam;

    void Start()
    {
        gameCam.enabled = false;
        menuCam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCameras()
    {
        gameCam.enabled = !gameCam.enabled;
        menuCam.enabled = !menuCam.enabled;
    }
}
