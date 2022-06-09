using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    float velMov;
    public float caminarVel;
    public float correrVel;
    public float slideVel;
    public float wallRunVel;

    public float multiplicarIncrementoVel;
    public float slopeMultiplicadorIncremento;
    public float groundDrag;
    
    float desiredMoveSpeed;
    float lastDesiredMoveSpeed;

    [Header("Salto")]
    public float fuerzaSalto;
    public float saltoCooldown;
    public float MultiplicadorEnAire;
    public bool puedeSaltar;
    public bool dobleSalto = false;

    [Header("Agacharse")]
    public float agachadoVel;
    public float scaleYAgachado; //para la altura
    float scaleYAgachdoI; //scale en el eje Y inicial para restaurar la altura

    [Header("Ground Check")]
    public float alturaJugador;
    public LayerMask piso;
    public LayerMask pared;
    bool grounded;

    [Header("Deslizamiento")]
    public float anguloMaxSlope;
    RaycastHit slopeHit;
    bool exitingSlope;
    public bool sliding;

    public bool wallRunning;

    public Transform orientacion;

    float inputHorizontal;
    float inputVertical;

    Vector3 direccionMov;

    Rigidbody rb;

    public MovementState estado;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        dobleSalto = true;
        puedeSaltar = true;
        scaleYAgachdoI = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, alturaJugador * 0.5f + 0.2f, piso);
        InputUsuario();
        ControlVel();
        StateHandler();

        if (grounded)
        {
            rb.drag = groundDrag;
            Invoke(nameof(ResetSalto), saltoCooldown);
            dobleSalto = true;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        MoverJugador();
    }

    private void InputUsuario()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && puedeSaltar && grounded && !wallRunning)
        {
            Saltar();
            puedeSaltar = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dobleSalto && !grounded && !wallRunning)
        {
            Saltar();
            dobleSalto = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && inputHorizontal == 0 && inputVertical == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, scaleYAgachado, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.C) && inputHorizontal == 0 && inputVertical == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, scaleYAgachdoI, transform.localScale.z);
        }
    }

    void StateHandler()
    {
        if (wallRunning)
        {
            estado = MovementState.wallrunning;
            desiredMoveSpeed = wallRunVel;
        }

        if (sliding)
        {
            estado = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideVel;
            }
            else
            {
                desiredMoveSpeed = correrVel;
            }
        }
        else if (Input.GetKey(KeyCode.C))
        {
            estado = MovementState.crouching;
            desiredMoveSpeed = agachadoVel;
        }
        else if (grounded && Input.GetKey(KeyCode.LeftShift))
        {
            estado = MovementState.sprinting;
            desiredMoveSpeed = correrVel;
        }
        else if (grounded)
        {
            estado = MovementState.walking;
            desiredMoveSpeed = caminarVel;
        }
        else
        {
            estado = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && velMov != 0) //si la velocidad deseada aumenta drasticamente disminuye el avance
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            velMov = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    IEnumerator SmoothlyLerpMoveSpeed()
    {
        float tiempo = 0;
        float diferencia = Mathf.Abs(desiredMoveSpeed - velMov);
        float valorInicial = velMov;

        while (tiempo < diferencia)
        {
            velMov = Mathf.Lerp (valorInicial, desiredMoveSpeed, tiempo / diferencia);

            if (OnSlope())
            {
                float anguloSlope = Vector3.Angle(Vector3.up, slopeHit.normal);
                float incrementoAnguloSlope = 1 + (anguloSlope / 90f);
                tiempo += Time.deltaTime * multiplicarIncrementoVel * slopeMultiplicadorIncremento * incrementoAnguloSlope;
            }
            else
            {
                tiempo += Time.deltaTime * multiplicarIncrementoVel;
            }
            yield return null;
        }
        velMov = desiredMoveSpeed;
    }

    void MoverJugador()
    {
        direccionMov = orientacion.forward * inputVertical + orientacion.right * inputHorizontal;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(direccionMov) * velMov * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (grounded)
        {
            rb.AddForce(direccionMov.normalized * velMov * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(direccionMov.normalized * velMov * 10f * MultiplicadorEnAire, ForceMode.Force);
        }

        if (!wallRunning)
        {
            rb.useGravity = !OnSlope();
        }
    }

    void ControlVel()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > velMov)
            {
                rb.velocity = rb.velocity.normalized * velMov;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > velMov)
            {
                Vector3 limiteVel = flatVel.normalized * velMov;
                rb.velocity = new Vector3(limiteVel.x, rb.velocity.y, limiteVel.z);
            }
        }
    }

    void Saltar()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * fuerzaSalto, ForceMode.Impulse);
    }
    void ResetSalto()
    {
        puedeSaltar = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, alturaJugador * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < anguloMaxSlope && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
