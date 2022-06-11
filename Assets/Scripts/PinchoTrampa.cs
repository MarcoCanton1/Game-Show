using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchoTrampa : MonoBehaviour
{
    bool subiendo = false;

    void Update()
    {
        if (transform.position.y > -16.43f && subiendo == false)
        {
            transform.position -= new Vector3 (0, Time.deltaTime, 0);
        }
        else if (transform.position.y <= -16.43f)
        {
            subiendo = true;
        }

        if (transform.position.y < -13.62975f && subiendo)
        {
            transform.position += new Vector3(0, Time.deltaTime, 0);
        }
        else if (transform.position.y >= -13.62975f)
        {
            subiendo = false;
        }
    }
}
