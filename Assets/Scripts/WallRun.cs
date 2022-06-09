using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask Pared;
    public LayerMask piso;
    public float fuerzaWallRun;
    public float wallClimbVel;
    public float wallRunTiempoMax;
    public float fuerzaSaltoUpWallJump;
    public float fuerzaSaltoWallJump;
    float wallRunTimer;

    [Header("Input")]
    bool escalarUp;
    bool escalarDown;
    float inputHorizontal;
    float inputVertical;

    [Header("Detección")]
    public float distanciaPared;
    public float alturaSaltoMinima;
    RaycastHit leftWallhit;
    RaycastHit rightWallhit;
    bool muroIzquierdo;
    bool muroDerecho;

    [Header("Referencias")]
    public Transform orientacion;
    MovimientoJugador MJ;
    public JugadorCamara camara;
    Rigidbody rb;

    [Header("Gravedad")]
    public bool gravedad;
    public float gravedadCounterForce;

    bool DejandoPared;
    float dejarParedTimer;
    public float DejarParedTiempo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MJ = GetComponent<MovimientoJugador>();
    }

    void Update()
    {
        CheckPared();
        StateMachine();
    }

    void FixedUpdate()
    {
        if (MJ.wallRunning)
        {
            MovimientoWallRunning();
        }
    }

    void CheckPared() //se revisa con la orientacion de que lado esta el muro utilizando un raycast
    {
        muroDerecho = Physics.Raycast(transform.position, orientacion.right, out rightWallhit, distanciaPared, Pared);
        muroIzquierdo = Physics.Raycast(transform.position, -orientacion.right, out leftWallhit, distanciaPared, Pared);
    }

    bool sobreSuelo()
    {
        return !Physics.Raycast(transform.position, Vector3.down, alturaSaltoMinima, piso); //revisa si el jugador esta arriba del suelo para poder hacer wallrun
    }

     void StateMachine() //se revisa es estado del jugador en el script MovimientoJugador
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        escalarUp = Input.GetKey(KeyCode.LeftShift);
        escalarDown = Input.GetKey(KeyCode.LeftControl);

        if ((muroIzquierdo || muroDerecho) && inputVertical > 0 && sobreSuelo() && !DejandoPared) //en pared
        {
            if (!MJ.wallRunning)
            {
                IniciarWallRun();
            }

            if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }

            if(wallRunTimer <= 0 && MJ.wallRunning)
            {
                DejandoPared = true;
                dejarParedTimer = DejarParedTiempo;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }
        else if (DejandoPared) //dejar la pared
        {
            if (MJ.wallRunning)
            {
                StopWallRun();
            }

            if(dejarParedTimer > 0)
            {
                dejarParedTimer -= Time.deltaTime;
            }

            if(dejarParedTimer <= 0)
            {
                DejandoPared = false;
            }
        }
        else //nada
        {
            if (MJ.wallRunning)
            {
                StopWallRun();
            }
        }
    }

    void IniciarWallRun()
    {
        MJ.wallRunning = true;
        MJ.dobleSalto = true;
        dejarParedTimer = wallRunTiempoMax;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    void MovimientoWallRunning()
    {
        rb.useGravity = false;
        
        Vector3 paredNormal;
        if (muroDerecho)
        {
            paredNormal = rightWallhit.normal;
        }
        else
        {
            paredNormal = leftWallhit.normal;
        }
        Vector3 paredForward = Vector3.Cross(paredNormal, transform.up);

        if ((orientacion.forward - paredForward).magnitude > (orientacion.forward - -paredForward).magnitude)
        {
            paredForward = -paredForward;
        }
        rb.AddForce(paredForward * fuerzaWallRun, ForceMode.Force); //empuje para adelante

        if (escalarUp) //fuerza para subir
        {
            rb.velocity = new Vector3(rb.velocity.x, wallClimbVel, rb.velocity.z);
        }
        else if (escalarDown) //fuerza para bajar
        {
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbVel, rb.velocity.z);
        }

        //mantiene la fuerza o empuje al muro
        if (!(muroIzquierdo && inputHorizontal > 0) && !(muroDerecho && inputHorizontal < 0))
        {
            rb.AddForce(-paredNormal * 100, ForceMode.Force);
        }

        if (gravedad) //debilita la gravedad para facilitarlo
        {
            rb.AddForce(transform.up * gravedadCounterForce, ForceMode.Force);
        }
    }

    void StopWallRun()
    {
        MJ.wallRunning = false;
    }

    void WallJump()
    {
        DejandoPared = true;
        dejarParedTimer = DejarParedTiempo;

        Vector3 paredNormal;
        if (muroDerecho)
        {
            paredNormal = rightWallhit.normal;
        }
        else
        {
            paredNormal = leftWallhit.normal;
        }

        Vector3 fuerzaAplicable = transform.up * fuerzaSaltoUpWallJump + paredNormal * fuerzaSaltoWallJump;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(fuerzaAplicable, ForceMode.Impulse);
    }
}
