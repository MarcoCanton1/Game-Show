using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    public bool apreto = false;
    public GameObject pared;
    void Start()
    {
        
    }

    void Update()
    {
        if (apreto)
        {
            if (pared.transform.position.y < 24.5f)
            {
                pared.transform.position += new Vector3(0, 0.2f, 0);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Jugador")
        {
            apreto = true;
        }
    }
}
