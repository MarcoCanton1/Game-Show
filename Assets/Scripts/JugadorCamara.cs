using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorCamara : MonoBehaviour
{
    public float sensibilidadEjeX;
    public float sensibilidadEjeY;
    public Transform orientacion;
    public Transform HolderCamara;
    float rotacionX;
    float rotacionY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Input del mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensibilidadEjeX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensibilidadEjeY;

        rotacionY += mouseX;
        rotacionX -= mouseY;
        rotacionX= Mathf.Clamp(rotacionX, -90f, 90f);

        //rotacion del mouse
        HolderCamara.rotation = Quaternion.Euler(rotacionX, rotacionY, 0);
        orientacion.rotation = Quaternion.Euler(0, rotacionY, 0);
    }
}
