using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using Windows.Kinect;
//using Kinect = Windows.Kinect;

public class BodyReader : MonoBehaviour
{
    public GameObject ShoulderLeft;
    public GameObject ShoulderRight;

    public Vector3 ShoulderLeftPos;
    public Vector3 ShoulderRightPos;

    //Variable que comprueba cuando la kinect comienza a detectar el cuerpo
    [SerializeField]
    private bool lastWasNull = true;
    [SerializeField]
    private bool lastWasNull2 = true;

    //Variable para asegurarse de que el jugador está en el suelo y evitar posibles saltos solapados
    [SerializeField]
    private bool isGrounded = true;

    //Variable para asegurarse de que el jugador está recto y evitar que se esté moviendo infinitamente 
    [SerializeField]
    private bool StandRight = true;
    [SerializeField]
    private bool StandLeft = true;

    public float InitialYPositionLeft;
    public float InitialYPositionRight;
    public float InitialXPosition;

    //Rango que definirá cuando se interpreta que el jugador saltó
    public float JumpThreshold;

    //Rango que definirá cuando el jugador se ha inclinado a cualquiera de los dos lados
    public float RightThreshold;
    public float LeftThreshold;

    //Tiempo que se le deja al jugador para colocarse bien
    public float RecordTime = 1.0f;

    //Eventos que se mostrarán en el inspector para llamar a las distintas funciones
    //public UnityEvent onJump;
    public UnityEvent onRight;
    public UnityEvent stopRight;
    public UnityEvent onLeft;
    public UnityEvent stopLeft;

    private int contador = 0;

    //public Windows.Kinect.Body body;

    private void Update()
    {
        //Buscar el hombro izquierdo como referencia
        if (ShoulderLeft == null)
        {
            ShoulderLeft = GameObject.Find("ShoulderLeft");
        }
        if (ShoulderRight == null)
        {
            ShoulderRight = GameObject.Find("ShoulderRight");
        }

        //En cuanto la kinect detecte el esqueleto, llamará a la función para identificar la posición inicial base
        if (lastWasNull && (ShoulderLeft != null))
        {
            Invoke("RecordInitialPosition", RecordTime);
        }

        //Aquí, una vez se detecta el esqueleto, se calcula la posición del hombro izquierdo y dereho (deteción salto)
        if (ShoulderLeft != null)
        {
            ShoulderLeftPos = ShoulderLeft.transform.position;
            lastWasNull = false;
        }
        else
        {
            ShoulderLeftPos = Vector3.zero;
            lastWasNull = true;
        }

        if (ShoulderRight != null)
        {
            ShoulderRightPos = ShoulderRight.transform.position;
            lastWasNull2 = false;
        }
        else
        {
            ShoulderRightPos = Vector3.zero;
            lastWasNull2 = true;
        }

        //Finalmente se comprueba que el jugador, estando en el suelo, ha saltado, evitando el caso concreto de la primera aparición del esqueleto
        /* De momento no se saltará por lo que esta parte del codigo queda comentada
        if (isGrounded && ((ShoulderLeftPos.y - InitialYPosition) > JumpThreshold) && (InitialYPosition != 0) && ((ShoulderRightPos.y - InitialYPosition2) > JumpThreshold) && (InitialYPosition != 0))
        {
            onJump.Invoke();
            isGrounded = false;
        }
        else if (!isGrounded && ((ShoulderLeftPos.y - InitialYPosition) < JumpThreshold) && ((ShoulderRightPos.y - InitialYPosition) < JumpThreshold))
        {
            isGrounded = true;
        }
        */

        //Ahora se realizará lo mismo para realizar los movimientos de izquierda y derecha
        //Derecha
        if (StandRight && ((ShoulderLeftPos.y - InitialYPositionLeft) > RightThreshold) && (InitialYPositionLeft != 0) && contador == 0)
        {
            onRight.Invoke();
            StandRight = false;
            contador = 1;
        }
        else if (!StandRight && ((ShoulderLeftPos.y - InitialYPositionLeft) < RightThreshold))
        {
            stopRight.Invoke();
            StandRight = true;
            contador = 0;
        }
        //Izquierda
        if (StandLeft && ((ShoulderLeftPos.y - InitialYPositionLeft) < LeftThreshold) && (InitialYPositionLeft != 0) && contador == 0)
        {
            onLeft.Invoke();
            StandLeft = false;
            contador = 1;
        }
        else if (!StandLeft && ((ShoulderLeftPos.y - InitialYPositionLeft) > LeftThreshold))
        {
            stopLeft.Invoke();
            StandLeft = true;
            contador = 0;
        }
    }

    void CheckDirection(Vector3 shoulderRightPos, Vector3 shoulderLeftPos, float threshold)
    {
        //Tendremos que detectar ambos hombros. Si uno esta mas bajo (eje Y) que otro se va en la direccion del mas bajo (aplicar un threshold)
        if (Mathf.Abs(shoulderRightPos.y - shoulderLeftPos.y) > threshold)
        {
            //esto significa que el Left esta mas alto, por lo tanto vamos a la derecha
        }

    }

    //Aquí se guardarán las coordenadas de referencia iniciales
    private void RecordInitialPosition()
    {
        isGrounded = true;
        InitialYPositionLeft = ShoulderLeftPos.y;
        InitialYPositionRight = ShoulderRightPos.y;
        InitialXPosition = ShoulderLeftPos.x;
    }
}