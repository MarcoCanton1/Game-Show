using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPos : MonoBehaviour
{
    public Transform posicionCamara;

    void Update()
    {
        transform.position = posicionCamara.position;
    }
}
