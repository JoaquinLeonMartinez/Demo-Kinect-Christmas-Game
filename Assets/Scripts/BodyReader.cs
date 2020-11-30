using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using Windows.Kinect;
//using Kinect = Windows.Kinect;

public class BodyReader : MonoBehaviour
{
    //public GameObject ShoulderLeft;
    //public GameObject ShoulderRight;

    //public Vector3 ShoulderLeftPos;
    //public Vector3 ShoulderRightPos;

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
    //public float RightThreshold;
    public float Threshold = 0;//0.01f;
    //public float LeftThreshold = 0.1f;

    //Tiempo que se le deja al jugador para colocarse bien
    public float RecordTime = 1.0f;

    public float timeWaiting;
    public bool userDetected;

    //Eventos que se mostrarán en el inspector para llamar a las distintas funciones
    //public UnityEvent onJump;
    public UnityEvent onRight;
    public UnityEvent stopRight;
    public UnityEvent onLeft;
    public UnityEvent stopLeft;

    private int contador = 0;


    public GameObject HandLeft;
    public GameObject HandRight;

    public Vector3 HandLeftPos;
    public Vector3 HandRightPos;

    [SerializeField] Controll controll;
    public float rotationLimit;
    /*
     HandLeft
     HandRight
     */
    //public Windows.Kinect.Body body;

    private void Update()
    {
        //Buscar el hombro izquierdo como referencia
        if (HandLeft == null && HandRight == null)
        {
            //ShoulderLeft = GameObject.Find("HandLeft"); //TODO: esto vamos a tener que cambiarlo (UNA OPCION ES PARAMETRIZAR ESTO, EL PROBLEMA ES QUE ELESQUELETO SE GENERA EN TIEMPO REAL)
            HandLeft = GameObject.Find("HandLeft");
            HandRight = GameObject.Find("HandRight");
        }
        /*
        if (ShoulderRight == null)
        {
            ShoulderRight = GameObject.Find("ShoulderRight");
        }
        */
        //En cuanto la kinect detecte el esqueleto, llamará a la función para identificar la posición inicial base
        if (lastWasNull && (HandLeft != null)) //Con el lastWasNull limita que solo haya uno, que es el primero que pilla (esto en realidad no esta tan mal) ademas lo recalcula por cada verz que entra en pantalla uno nuevo
        {
            Invoke("RecordInitialPosition", RecordTime); //en lugar de llamar directamente al metodo espera un segundo para que el jugador se colque correctamente
        }

        //Aquí, una vez se detecta el esqueleto, se calcula la posición del hombro izquierdo y dereho (deteción salto)
        if (HandLeft != null)
        {
            HandLeftPos = HandLeft.transform.position;
            HandRightPos = HandRight.transform.position;
            userDetected = ChechPositionInit();
            timeWaiting = 0;
            lastWasNull = false;
        }
        else
        {
            userDetected = false;
            timeWaiting += Time.deltaTime;
            HandLeftPos = Vector3.zero;
            HandRightPos = Vector3.zero;
            lastWasNull = true;
        }
        
        if (userDetected && HandLeftPos.y > (HandRightPos.y + Threshold))//HandLeftPos.y > (InitialYPositionLeft + Threshold
        {
            //float difference =  HandLeftPos.y > (HandRightPos.y + Threshold);
            
            //giramos a la derecha en este caso
            //onRight.Invoke();
            float rotation = HandLeftPos.y - (HandRightPos.y + Threshold);
            if (rotation > rotationLimit)
            {
                rotation = rotationLimit;
            }
            stopLeft.Invoke();
            controll.MoveRight(rotation * 0.4f);

        }
        else if (userDetected && HandLeftPos.y < (HandRightPos.y - Threshold))
        {
            
            //giramos a la izq
            //onLeft.Invoke();
            float rotation = (HandRightPos.y - Threshold) - HandLeftPos.y;
            if (rotation > rotationLimit)
            {
                rotation = rotationLimit;
            }
            stopRight.Invoke();
            controll.MoveLeft(rotation*0.4f);
        }
        /*else if(userDetected && HandLeftPos.y > (HandRightPos.y - Threshold) && HandLeftPos.y < (HandRightPos.y + Threshold))
        {
            Debug.Log("Aqui no deberia entrar, significa que va recto");
            stopLeft.Invoke();
            stopRight.Invoke();
        }*/
    }

    public bool ChechPositionInit()
    {
        if (GameManager.Instance.playing) //si ya esta jugando y lo pierde un momento no hace falta que haga la posicion
        {
            return true;
        }

        GameObject Head = GameObject.Find("Head");

        //comprobar que la cabeza este por debajo de ambas manos
        if (Head != null && Head.transform.position.y < HandLeftPos.y && Head.transform.position.y < HandRightPos.y)
        {
            return true;
        }

        return false;
    }

    //Aquí se guardarán las coordenadas de referencia iniciales
    private void RecordInitialPosition()
    {
        //isGrounded = true; //al no haber salto esto nos da igual
        InitialYPositionLeft = HandLeftPos.y;
        InitialYPositionRight = HandRightPos.y;
        InitialXPosition = HandLeftPos.x;
    }
}