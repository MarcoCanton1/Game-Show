using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawncajas : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spawner;
    bool existe = false;
    public GameObject clon;

    void Start()
    {
        
    }

    void Update()
    {
        if (!existe)
        {
            clon = Instantiate(prefab, spawner.transform.position, spawner.transform.rotation);
            existe = true;
            StartCoroutine(reiniciar());
        }
    }

    IEnumerator reiniciar()
    {
        yield return new WaitForSeconds(3f);
        Destroy(clon);
        existe = false;
    }
}
