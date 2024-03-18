using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ficha : MonoBehaviour
{
    // Este script representa una ficha de jugador
    // Cada ficha tiene un dado, una casilla actual y un booleano que indica si está en movimiento
    // El dado se utiliza para mover la ficha
    // La casilla actual es el índice de la casilla en la que se encuentra la ficha
    // El booleano se activa cuando la ficha se está moviendo
    // La ficha se mueve por el tablero hasta llegar a la casilla destino

    public Dado dado; // Dado

    public int casillaActual; // Casilla actual de la ficha

    public bool enMovimiento = false; // Indica si la ficha está en movimiento

    public Tablero tablero; // Tablero

    public Text textoCasillaActual; // Texto de la casilla actual

    // Start is called before the first frame update
    void Start()
    {
        casillaActual = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Nombre del objeto + casilla actual
        textoCasillaActual.text = gameObject.name + ": " + casillaActual;
    }

    // Moverse hasta llegar a la casilla destino
    public void Mover(int indiceCasillaDestino)
    {
        if (!enMovimiento)
        {
            Casilla casillaDestino =
                tablero.ObtenerCasillaPorIndice(indiceCasillaDestino);

            if (casillaDestino != null)
            {
                StartCoroutine(MoverCoroutine(casillaDestino));
            }
        }
    }

    // Corrutina para moverse
    private IEnumerator MoverCoroutine(Casilla casillaDestino)
    {
        enMovimiento = true;

        Vector3 posicionDestino = casillaDestino.ObtenerPosicion();

        // Moverse hasta la casilla destino
        while (transform.position != posicionDestino)
        {
            Vector3 posicionSiguiente =
                tablero
                    .ObtenerCasillaPorIndice(casillaActual + 1)
                    .ObtenerPosicion();
            while (transform.position != posicionSiguiente)
            {
                transform.position =
                    Vector3
                        .MoveTowards(transform.position,
                        posicionSiguiente,
                        5 * Time.deltaTime);
                yield return null;
            }
            casillaActual++;
        }

        // Actualizar la casilla actual
        enMovimiento = false;
    }
}
