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

    public int casillaActual = 0; // Casilla actual de la ficha

    public bool enMovimiento = false; // Indica si la ficha está en movimiento

    public Tablero tablero; // Tablero

    public Text textoCasillaActual; // Texto de la casilla actual

    private GestorDeTurnos gestorDeTurnos; // Gestor de turnos

    // Start is called before the first frame update
    void Start()
    {
        // Obtener la casilla actual del EstadoDeEscena
        // dependiendo del nombre del objeto
        if (gameObject.name == "J1")
        {
            casillaActual = EstadoDeEscena.ObtenerInstancia().casillaJ1;
        }
        else if (gameObject.name == "J2")
        {
            casillaActual = EstadoDeEscena.ObtenerInstancia().casillaJ2;
        }
        MoverRapido();

        // Buscar por nombre el objeto GestorDeTurnos
        gestorDeTurnos =
            GameObject.Find("GestorDeTurnos").GetComponent<GestorDeTurnos>();
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

    public void MoverPatras(int indiceCasillaDestino)
    {
        if (!enMovimiento)
        {
            Casilla casillaDestino =
                tablero.ObtenerCasillaPorIndice(indiceCasillaDestino);

            if (casillaDestino != null)
            {
                StartCoroutine(MoverPatrasCoroutine(casillaDestino));
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
            EnviarAEstadoDeEscena();
        }

        // Actualizar la casilla actual
        enMovimiento = false;

        // Comprobar si la casilla tiene un evento
        gestorDeTurnos.ComprobarEvento (casillaActual);
    }

    // Corrutina para moverse
    private IEnumerator MoverPatrasCoroutine(Casilla casillaDestino)
    {
        enMovimiento = true;
        Vector3 posicionDestino = casillaDestino.ObtenerPosicion();

        // Moverse hasta la casilla destino
        while (transform.position != posicionDestino)
        {
            Vector3 posicionSiguiente =
                tablero
                    .ObtenerCasillaPorIndice(casillaActual - 1)
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
            casillaActual--;
            EnviarAEstadoDeEscena();
        }

        // Actualizar la casilla actual
        enMovimiento = false;
    }

    private void MoverRapido()
    {
        Vector3 posicionDestino =
            tablero.ObtenerCasillaPorIndice(casillaActual).ObtenerPosicion();
        transform.position = posicionDestino;
    }

    // Enviar la casilla actual al EstadoDeEscena
    private void EnviarAEstadoDeEscena()
    {
        if (gameObject.name == "J1")
        {
            EstadoDeEscena.ObtenerInstancia().casillaJ1 = casillaActual;
        }
        else if (gameObject.name == "J2")
        {
            EstadoDeEscena.ObtenerInstancia().casillaJ2 = casillaActual;
        }
    }
}
