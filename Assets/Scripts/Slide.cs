using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [Header("Referencia")]
    public Transform orientacion;
    public Transform playerObj;
    Rigidbody rb;
    MovimientoJugador MJ;

    [Header("Sliding")]
    public float tiempoMaxSlide;
    public float fuerzaSlide;
    float slideTimer;

    public float scaleYSlide;
    float scaleYSlideI; //la I marca que es inicial, en tras palabras es el scale en el eje Y inicial

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    float inputHorizontal;
    float inputVertical;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MJ = GetComponent<MovimientoJugador>();
        scaleYSlideI = playerObj.localScale.y;
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (inputHorizontal != 0 || inputVertical != 0))
        {
            StartSlide();
        }
        
        if (Input.GetKeyUp(slideKey) && MJ.sliding)
        {
            StopSlide();
        }
    }

    void FixedUpdate()
    {
        if (MJ.sliding)
        {
            MovSlide();
        }
    }

    void StartSlide()
    {
        MJ.sliding = true;
        playerObj.localScale = new Vector3(playerObj.localScale.x, scaleYSlide, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        slideTimer = tiempoMaxSlide;
    }

    private void MovSlide()
    {
        Vector3 inputDirection = orientacion.forward * inputVertical + orientacion.right * inputHorizontal;

        if (!MJ.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * fuerzaSlide, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(MJ.GetSlopeMoveDirection(inputDirection) * fuerzaSlide, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        MJ.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, scaleYSlideI, playerObj.localScale.z);
    }
}
 