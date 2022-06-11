using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Perderrr : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void reintentar()
    {
        SceneManager.LoadScene("Nivel");
    }

    public void salir()
    {
        SceneManager.LoadScene("Menu");
    }
}
