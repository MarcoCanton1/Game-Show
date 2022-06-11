using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JeroMuro : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.position -= new Vector3 (0, 0, Time.deltaTime*4);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "jugador")
        {
            SceneManager.LoadScene("Perder");
        }
    }
}
