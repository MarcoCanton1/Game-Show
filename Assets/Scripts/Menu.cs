using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Image fondoDefault;
    public Image panelConfig;
    public Dropdown fondo;
    public AudioSource musicaSource;
    public float volumenMusica;
    public Slider volumen;
    public GameObject amongus;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        panelConfig.gameObject.SetActive(false);
        musicaSource = GetComponent<AudioSource>();
        for (int i = 0; i <= 10; i++)
        {
            GameObject clon = Instantiate(amongus);
            Destroy(clon, 3f);
        }
    }

    void Update()
    {
        if (fondo.value == 1)
        {
            fondoDefault.gameObject.SetActive(false);
        }
        else
        {
            fondoDefault.gameObject.SetActive(true);
        }
        musicaSource.volume = volumen.value;
        volumenMusica = volumen.value;

    }

    public void Iniciar()
    {
        SceneManager.LoadScene("tutorial");
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void config()
    {
        panelConfig.gameObject.SetActive(true);
    }

    public void cerrarConfig()
    {
        panelConfig.gameObject.SetActive(false);
    }
}
