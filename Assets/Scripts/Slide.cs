using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [Header("Referencias")]
    public Transform orientacion;
    public Transform JugadorObj;
    Rigidbody rb;
    MovimientoJugador Mov;

    [Header("Deslizarse")]
    public float fuerza;
    float tiempoSlide;

    public float ScaleYSlide;
    float ScaleYSlideI;

    float InputHorizontal;
    float InputVertical;

    bool Sliding;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Mov = GetComponent<MovimientoJugador>();

        ScaleYSlideI = JugadorObj.localScale.y;
    }

    void SlideIniciado()
    {
        Sliding = true;
        JugadorObj.localScale = new Vector3(JugadorObj.localScale.x, ScaleYSlide, JugadorObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }
    void SlideFin()
    {
        Sliding = false;
        JugadorObj.localScale = new Vector3(JugadorObj.localScale.x, ScaleYSlideI, JugadorObj.localScale.z);
    }
    void MovSlide()
    {
        Vector3 InputDireccion = orientacion.forward * InputVertical + orientacion.right * InputHorizontal;
        rb.AddForce(InputDireccion.normalized * fuerza, ForceMode.Impulse);
    }

    void Update()
    {
        InputHorizontal = Input.GetAxisRaw("Horizontal");
        InputVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl) && (InputHorizontal != 0 || InputVertical != 0))
        {
            SlideIniciado();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && Sliding)
        {
            SlideFin();
        }
    }

    void FixedUpdate()
    {
        if (Sliding)
        {
            MovSlide();
        }
    }
}
 