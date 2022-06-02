using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    Text velocidadTxt;
    public int resolucion;
    public Rigidbody rb;

    void Start()
    {
        velocidadTxt = GetComponentInChildren<Text>();
    }

    void Update()
    {
        velocidadTxt.text = ((Mathf.Floor(rb.velocity.magnitude * resolucion) / resolucion)).ToString(); 
    }
}
