using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SigNivel : MonoBehaviour
{
    public GameObject jugador;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Jugador")
        {
            SceneManager.LoadScene("Nivel");
        }
    }
}
