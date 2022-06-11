using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlVol : MonoBehaviour
{
    public AudioSource musica;
    Menu menu;

    void Start()
    {
        musica.GetComponent<AudioSource>();
        musica.volume = menu.volumenMusica;
    }

    void Update()
    {

    }
}
