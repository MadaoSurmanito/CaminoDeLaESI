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

    public bool pierdeTurno = false; // Indica si el jugador pierde el turno

    public bool bloqueoPozo = false; // Indica si el jugador está en el pozo

    public int contadorTurnosPerdidos = 0; // Contador de turnos perdidos

    public int creditos = 0; // Créditos del jugador

    public Tablero tablero; // Tablero

    public Text textoCasillaActual; // Texto de la casilla actual

    private GestorDeTurnos gestorDeTurnos; // Gestor de turnos

    // Start is called before the first frame update
    void Start()
    {
        // Rescatar el estado previo
        GetEstadoEscena();

        // Esto hace que se muevan a la posición correspondiente cada vez que se carga la escena
        MoverRapido();

        // Buscar por nombre el objeto GestorDeTurnos
        gestorDeTurnos = GameObject.Find("GestorDeTurnos").GetComponent<GestorDeTurnos>();
    }

    void GetEstadoEscena()
    {
        if (gameObject.name == "J1")
        {
            casillaActual = EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].casilla;
            pierdeTurno = EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].pierdeTurno;
            bloqueoPozo = EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].bloqueoPozo;
            contadorTurnosPerdidos = EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].contadorTurnosPerdidos;
            creditos = EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].creditos;
        }
        else if (gameObject.name == "J2")
        {
            casillaActual = EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].casilla;
            pierdeTurno = EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].pierdeTurno;
            bloqueoPozo = EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].bloqueoPozo;
            contadorTurnosPerdidos = EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].contadorTurnosPerdidos;
            creditos = EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].creditos;
        }
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
            Casilla casillaDestino = tablero.ObtenerCasillaPorIndice(indiceCasillaDestino);

            if (casillaDestino != null)
            {
                StartCoroutine(MoverCoroutine(casillaDestino));
            }
        }
    }

    // Moverse hacia atrás hasta llegar a la casilla destino
    public void MoverPatras(int indiceCasillaDestino)
    {
        if (!enMovimiento)
        {
            Casilla casillaDestino = tablero.ObtenerCasillaPorIndice(indiceCasillaDestino);

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
            Vector3 posicionSiguiente = tablero.ObtenerCasillaPorIndice(casillaActual + 1).ObtenerPosicion();
            while (transform.position != posicionSiguiente)
            {
                transform.position = Vector3.MoveTowards(transform.position, posicionSiguiente, 5 * Time.deltaTime);
                yield return null;
            }
            casillaActual++;
            EnviarAEstadoDeEscena();
        }

        // Actualizar la casilla actual
        enMovimiento = false;

        // Comprobar si la casilla tiene un evento
        gestorDeTurnos.ComprobarEvento(this);
    }

    // Corrutina para moverse hacia atrás
    private IEnumerator MoverPatrasCoroutine(Casilla casillaDestino)
    {
        enMovimiento = true;
        Vector3 posicionDestino = casillaDestino.ObtenerPosicion();

        // Moverse hasta la casilla destino
        while (transform.position != posicionDestino)
        {
            Vector3 posicionSiguiente = tablero.ObtenerCasillaPorIndice(casillaActual - 1).ObtenerPosicion();
            while (transform.position != posicionSiguiente)
            {
                transform.position = Vector3.MoveTowards(transform.position, posicionSiguiente, 5 * Time.deltaTime);
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
        Vector3 posicionDestino = tablero.ObtenerCasillaPorIndice(casillaActual).ObtenerPosicion();
        transform.position = posicionDestino;
    }

    // Enviar el estado de la ficha actual al EstadoDeEscena
    public void EnviarAEstadoDeEscena()
    {
        if (gameObject.name == "J1")
        {
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].casilla = casillaActual;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].pierdeTurno = pierdeTurno;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].bloqueoPozo = bloqueoPozo;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].contadorTurnosPerdidos = contadorTurnosPerdidos;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[0].creditos = creditos;
        }
        else if (gameObject.name == "J2")
        {
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].casilla = casillaActual;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].pierdeTurno = pierdeTurno;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].bloqueoPozo = bloqueoPozo;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].contadorTurnosPerdidos = contadorTurnosPerdidos;
            EstadoDeEscena.ObtenerInstancia().valoresJugadores[1].creditos = creditos;
        }
    }
}
