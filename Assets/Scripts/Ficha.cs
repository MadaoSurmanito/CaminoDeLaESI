using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour
{
    public Dado dado;

    public int casillaActual;

    // Variable para controlar el movimiento de la ficha
    private bool enMovimiento = false;

    // Referencia al tablero
    public Tablero tablero;

    // Start is called before the first frame update
    void Start()
    {
        casillaActual = 0;
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
            Vector3 posicionSiguiente = tablero.ObtenerCasillaPorIndice(casillaActual + 1).ObtenerPosicion();
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
