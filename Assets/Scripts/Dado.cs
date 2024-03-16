using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dado : MonoBehaviour
{
    public Text texto;

    public Cara[] caras;

    public int numeroDado;

    public bool enElAire;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void NumeroDado()
    {
        // Recorre todas las caras
        for (int i = 0; i < caras.Length; i++)
        {
            // Si la cara toca el suelo
            if (caras[i].TocaSuelo)
            {
                // Esperar a que el dado se asiente en ese numero
                // El número del dado es el número de la cara
                // Es necesario restarlo a 7 porque la suma de las caras opuestas es 7
                numeroDado = 7 - caras[i].numeroCara;
                enElAire = false;
                // Muestra el número del dado
                texto.text = "Número: " + numeroDado;
            }
        }
    }

    public void LanzarDado()
    {
        if (!enElAire)
        {
            numeroDado = 0;
            enElAire = true;

            // Lanza el dado
            float fuerzaInicial = Random.Range(6, 15); // Fuerza inicial del lanzamiento
            GetComponent<Rigidbody>().isKinematic = false; // Activa la física para que el dado caiga

            // Rotación inicial del dado
            // MENSAJE PARA EL FUTURO
            // Esto hay que toquetearlo
            Quaternion rotacionInicial =
                new Quaternion(Random.Range(-100, 100),
                    Random.Range(-100, 100),
                    Random.Range(-100, 100),
                    1);

            // Aplica la fuerza y la rotación inicial
            GetComponent<Rigidbody>()
                .AddForce(Vector3.up * fuerzaInicial, ForceMode.Impulse);

            // Aplica la rotación inicial
            GetComponent<Rigidbody>().AddTorque(rotacionInicial.eulerAngles);

            // Espera a que el dado se asiente
            StartCoroutine(EsperarAQueSeAsiente());
        }
    }

    private IEnumerator EsperarAQueSeAsiente()
    {
        // Espera 1 segundo
        yield return new WaitForSeconds(1);

        // Mientras el dado esté en el aire
        while (enElAire)
        {
            // Comprueba el número del dado
            NumeroDado();
            yield return null;
        }
    }
}


