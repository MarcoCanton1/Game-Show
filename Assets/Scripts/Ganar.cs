using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ganar : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "jugador")
        {
            SceneManager.LoadScene("Ganar");
        }
    }
}
