using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    private float velMov;
    public float caminarVel;
    public float correrVel;
    public float alturaJugador;
    public float groundDrag;

    [Header("Salto")]
    public float fuerzaSalto;
    public float saltoCooldown;
    public float multiplicadorAire;

    [Header("Agacharse")]
    public float agacharseVel;
    public float scaleAgachado;
    float scaleAgachadoI;

    public Transform orientacion;

    public LayerMask piso;

    public MovementState state;

    float inputHorizontal;
    float inputVertical;

    bool isOnGround;
    bool canJump;

    int saltos;

    Vector3 direccionMov;

    Rigidbody rb;

    public enum MovementState
    {
        caminando,
        corriendo,
        agachado,
        aire
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        scaleAgachadoI = transform.localScale.y;
    }


    void Update()
    {
        Ingreso();
        HandlerEstado();
        isOnGround = Physics.Raycast(transform.position, Vector3.down, alturaJugador * 0.5f + 0.2f, piso);
        if (isOnGround)
        {
            rb.drag = groundDrag;
            Invoke(nameof(resetSalto), saltoCooldown);
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        Moverse();
    }

    void Ingreso() //void para el ingreso de teclas
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && canJump == true && saltos > 0)
        {
            saltos--;
            saltar();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, scaleAgachado, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, scaleAgachadoI, transform.localScale.z);
        }
    }

    void Moverse()
    {
        direccionMov = orientacion.forward * inputVertical + orientacion.right * inputHorizontal;
        if (isOnGround)
        {
            rb.AddForce(direccionMov.normalized * velMov * 10f, ForceMode.Force);
        }
        else if (!isOnGround)
        {
            rb.AddForce(direccionMov.normalized * velMov * 10f * multiplicadorAire, ForceMode.Force);
        }
        
    }

    void controlVel()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > velMov)
        {
            Vector3 limite = flatVel.normalized * velMov;
            rb.velocity = new Vector3(limite.x, rb.velocity.y, limite.z);
        }
    }

    void saltar()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * fuerzaSalto, ForceMode.Impulse);
    }

    void resetSalto()
    {
        canJump = true;
        saltos = 1;
    }

    void HandlerEstado()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            state = MovementState.agachado;
            velMov = agacharseVel;
        }

        if(isOnGround && Input.GetKey(KeyCode.LeftShift))
        {
            state = MovementState.corriendo;
            velMov = correrVel;
        }
        else if (isOnGround)
        {
            state = MovementState.caminando;
            velMov = caminarVel;
        }
        else
        {
            state = MovementState.aire;
        }
    }
}
