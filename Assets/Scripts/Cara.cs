using UnityEngine;

public class Cara : MonoBehaviour
{
    // Este script representa una cara de un dado
    // Cada cara tiene un numero y un booleano que indica si toca el suelo
    // El booleano se activa si la cara toca el suelo durante 1 segundos
    public int numeroCara; // Numero de la cara

    public bool TocaSuelo = false; // Indica si se ha tocado el suelo durante 1 segundos

    private bool preTocaSuelo; // Variable que indica si la cara toca el suelo

    private float tiempoTocandoSuelo = 0f; // Tiempo transcurrido tocando el suelo

    // Start is called before the first frame update
    void Start()
    {
        // El numero de la cara es el nombre del objeto
        numeroCara = int.Parse(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (preTocaSuelo)
        {
            // Incrementar el tiempo transcurrido tocando el suelo
            tiempoTocandoSuelo += Time.deltaTime;

            // Verificar si se ha tocado el suelo durante 1 segundos
            if (tiempoTocandoSuelo >= 1f)
            {
                TocaSuelo = true;
            }
        }
        else
        {
            // Reiniciar el tiempo transcurrido si no se está tocando el suelo
            tiempoTocandoSuelo = 0f;
            TocaSuelo = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si toca el suelo se activa la variable preTocaSuelo
        if (other.gameObject.tag == "Suelo")
        {
            preTocaSuelo = true;
        }
    }

    // Si sale del suelo se desactiva la variable preTocaSuelo
    void OnTriggerExit(Collider other)
    {
        preTocaSuelo = false;
    }
}
