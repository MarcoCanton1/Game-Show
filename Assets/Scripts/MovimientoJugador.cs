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
    public float aireVel;

    [Header("Agacharse")]
    public float agacharseVel;
    public float scaleAgachado;
    float scaleAgachadoI;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

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
        controlVel();
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

        if(Input.GetKey(KeyCode.Space) && canJump == true && saltos > 0)
        {
            exitingSlope = true;
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
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * velMov * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
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
        if (OnSlope() && !exitingSlope)
        {
            Debug.Log("buenas");
            if (rb.velocity.magnitude > velMov)
            {
                rb.velocity = rb.velocity.normalized;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > velMov)
            {
                Vector3 limite = flatVel.normalized * velMov;
                rb.velocity = new Vector3(limite.x, rb.velocity.y, limite.z);
            }
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
        exitingSlope = false;
    }

    void HandlerEstado()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            state = MovementState.agachado;
            velMov = agacharseVel;
            canJump = false;
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
            velMov = aireVel;
        }
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, alturaJugador * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (state == MovementState.corriendo) {
                velMov = correrVel;
            }
            else if (state == MovementState.caminando)
            {
                velMov = caminarVel;
            }
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(direccionMov, slopeHit.normal).normalized;
    }
}
