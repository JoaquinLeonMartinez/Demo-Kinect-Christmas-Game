using System.Collections;
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
    public float velocity;

    Vector3 originalPos;

    //Movimiento Update
    [SerializeField]
    bool goRight = false;
    [SerializeField]
    bool goLeft = false;

    void Start()
    {
        //rb = player.GetComponent<Rigidbody>(); //No se usara de momento ya que no se puede saltar
        originalPos = player.transform.localPosition;
    }

    void Update()
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
            //rb.AddForce(Vector3.up * jumpForce); //TODO: qUITAR SI FINALMENTE NO HAY SALTO
            //animator.Play("05_A_Santa_jump_Sack");
        }

        //Ecuaciones para el movimiento
        if (goRight == true)
        {
            position = player.transform.localPosition;
            if (position.y < 3)
            {
                player.transform.localPosition = position + new Vector3(0, 3, 0) * Time.deltaTime * velocity;
            }
        }
        if (goLeft == true)
        {
            position = player.transform.localPosition;
            if (position.y > -3)
            {
                player.transform.localPosition = position + new Vector3(0, -3, 0) * Time.deltaTime * velocity;
            }
        }
    }

    //Función a la que se llama para ir a la derecha
    public void MoveRight()
    {
        goRight = true;
    }
    public void StopRight()
    {
        goRight = false;
    }

    //Función a la que se llama para ir a la izquierda
    public void MoveLeft()
    {
        goLeft = true;
    }
    public void StopLeft()
    {
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
    }
}

