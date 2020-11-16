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
    public float rotateSpeed;

    public float frontSpeed; //esta se combina con la del follower

    Vector3 originalPos;
    Quaternion originalRotation;

    [SerializeField] Follower follower;
    [SerializeField] GameObject santa;

    //Movimiento Update
    [SerializeField]
    bool goRight = false;
    [SerializeField]
    bool goLeft = false;

    void Start()
    {
        //rb = player.GetComponent<Rigidbody>(); //No se usara de momento ya que no se puede saltar
        originalPos = player.transform.localPosition;
        originalRotation = santa.transform.rotation;
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

        if (goRight == true)
        {
            player.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotateSpeed);

        }
        if (goLeft == true)
        {
            //comprobar que no gire mas de 90 grados con respecto a la direccion de la carretera
            player.transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotateSpeed);
        }

        

        //Debug.Log("La rotacion actual del camino es: " + follower.getActualPathRotation());
        //Debug.Log("La rotacion actual del jugador es: " + player.transform.rotation);
        //siempre se mueve hacia delante: (habra que comprobar que no se salga del espacio que tenemos que delimitar
        //Debug.Log("La posicion x del jugador es: " + player.transform.localPosition.x);
        //Debug.Log("La posicion x del path es: " + follower.getCurrentPathCenter().x);
        //player.transform.localPosition += player.transform.forward * Time.deltaTime * frontSpeed;
        //Debug.Log("La distancia entre ellos es " + (Mathf.Abs(player.transform.position.x - follower.getCurrentPathCenter().x)));
        //if (Mathf.Abs(player.transform.position.x - follower.getCurrentPathCenter().x) < 30)
        /*if (player.transform.localPosition.x < 3 || player.transform.localPosition.x > -3)
        {
            Debug.Log("Se esta moviendo");
            //player.transform.position += new Vector3(player.transform.forward.x, 0, 0) * Time.deltaTime * frontSpeed;
            //player.transform.localPosition += player.transform.forward * Time.deltaTime * frontSpeed;
        }
        */
        player.transform.localPosition += player.transform.forward * Time.deltaTime * frontSpeed;
        /*
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
        */
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
        santa.transform.rotation = originalRotation;
        follower.ResetFollower();
    }
}

