﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Kinect = Windows.Kinect;

public class Controll : MonoBehaviour
{
    //Player hará referencia al objeto que moverá todo y poseerá el Rigidbody
    public GameObject player;

    //Este será el Rigidbody del jugador al que se hará referencia
    Rigidbody rb; //TODO: qUITAR SI FINALMENTE NO HAY SALTO

    //Model hará referencia al modelo 3d del personaje
    public GameObject model;

    //Este es el animator del modelo 3d
    Animator animator;

    //Este vector se utilizará para realizar los distintos movimientos
    private Vector3 position;

    //Variable que define la fuerza del salto
    public float jumpForce;

    //Velocidad a la que se realizará el movimiento de transición
    public float rotateSpeed;

    public float frontSpeed; //esta se combina con la del follower

    Vector3 originalPos;
    Quaternion originalRotation;
    Quaternion originalRotationMesh;

    [SerializeField] Follower follower;
    [SerializeField] GameObject santa;

    //Movimiento Update
    [SerializeField]
    bool goRight = false;
    [SerializeField]
    bool goLeft = false;
    public float angleSpeed;

    [SerializeField] GameObject playerMesh;
    public float angleMaxZ;

    void Start()
    {
        angleMaxZ = 40f;
        angleSpeed = 1f;
        //rb = player.GetComponent<Rigidbody>(); //No se usara de momento ya que no se puede saltar
        originalPos = player.transform.localPosition;
        originalRotation = santa.transform.rotation;
        originalRotationMesh = playerMesh.transform.rotation;
    }

    void Update()
    {
        if (GameManager.Instance.playing)
        {
            //Controles de teclado que simulan las acciones de kinect
            //Derecha
            if (Input.GetKeyDown(KeyCode.D))
            {
                position = player.transform.localPosition;
                goRight = true;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                goRight = false;
            }
            //Izquierda
            if (Input.GetKeyDown(KeyCode.A))
            {
                position = player.transform.localPosition;
                goLeft = true;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                goLeft = false;
            }
            //Salto
            if (Input.GetKeyDown(KeyCode.W))
            {
                //rb.AddForce(Vector3.up * jumpForce); //TODO: QUITAR SI FINALMENTE NO HAY SALTO
                //animator.Play("05_A_Santa_jump_Sack");
            }

            if (goRight == true)
            {
                //Debug.Log($"Rotate speed = {rotateSpeed} - AngleSpeed = {angleSpeed}");
                player.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotateSpeed * angleSpeed);

                if (playerMesh.transform.localEulerAngles.z > (360 - angleMaxZ) || playerMesh.transform.localEulerAngles.z < 180)
                {
                    playerMesh.transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * rotateSpeed * angleSpeed);
                }
            }
            if (goLeft == true)
            {
                //comprobar que no gire mas de 90 grados con respecto a la direccion de la carretera

                //Debug.Log($"Rotate speed = {rotateSpeed}- AngleSpeed = {angleSpeed}");
                player.transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotateSpeed * angleSpeed);

                if (playerMesh.transform.localEulerAngles.z < angleMaxZ || playerMesh.transform.localEulerAngles.z > 180)
                {
                    playerMesh.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * rotateSpeed * angleSpeed);
                }
            }
            player.transform.localPosition += player.transform.forward * Time.deltaTime * frontSpeed;
        }
    }

    //Función a la que se llama para ir a la derecha
    public void MoveRight(float angleSpeed)
    {
        this.angleSpeed = angleSpeed;
        goRight = true;
    }
    public void StopRight()
    {
        this.angleSpeed = 1f;
        goRight = false;
    }

    //Función a la que se llama para ir a la izquierda
    public void MoveLeft(float angleSpeed)
    {
        this.angleSpeed = angleSpeed;
        goLeft = true;
    }
    public void StopLeft()
    {
        this.angleSpeed = 1f;
        goLeft = false;
    }

    //Función a la que se llama para saltar
    public void Jump()
    {
        //rb.AddForce(Vector3.up * jumpForce); //TODO: qUITAR SI FINALMENTE NO HAY SALTO
    }

    public void ResetControll()
    {
        player.transform.localPosition = originalPos;
        santa.transform.rotation = originalRotation;
        follower.ResetFollower();
        playerMesh.transform.rotation = originalRotationMesh;
        goRight = false;
        goLeft = false;
    }
}

