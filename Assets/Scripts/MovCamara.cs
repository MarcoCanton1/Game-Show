using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovCamara : MonoBehaviour
{
    public float sensibilidadEjeX;
    public float sensibilidadEjeY;
    float rotacionEjeX;
    float rotacionEjeY;
    public Transform orientacion;


    void Start()
    {
        //bloqueo de cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        //Input del mouse
        float EjeX = Input.GetAxis("Mouse X") * Time.deltaTime * sensibilidadEjeX;
        float EjeY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensibilidadEjeY;

        rotacionEjeX -= EjeY;
        rotacionEjeX = Mathf.Clamp(rotacionEjeX, -90f, 90f);
        rotacionEjeY += EjeX;

        //rotacion del mouse
        transform.rotation = Quaternion.Euler(rotacionEjeX, rotacionEjeY, 0);
        orientacion.rotation = Quaternion.Euler(0, rotacionEjeY, 0);
    }
}
