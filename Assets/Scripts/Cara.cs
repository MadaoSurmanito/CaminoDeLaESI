using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cara : MonoBehaviour
{
    public int numeroCara;
    public bool TocaSuelo;
    // Start is called before the first frame update
    void Start()
    {
        // El numero de la cara es el nombre del objeto
        numeroCara = int.Parse(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Suelo")
        {
            TocaSuelo = true;
        }
    }

        void OnTriggerExit(Collider other)
    {
            TocaSuelo = false;
    }
}
