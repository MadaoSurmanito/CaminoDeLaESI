using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GestorDeTurnos : MonoBehaviour
{
    public Ficha
        // Fichas de los jugadores

            ficha1,
            ficha2;

    public Dado dado; // Dado

    public bool
        // Turnos de los jugadores

            turnoFicha1,
            turnoFicha2,
            tocaMinijuego;

    private bool
        // Variables para controlar los eventos de las casillas

            ocaPrevia = false,
            puentePrevio = false,
            dadoPrevio = false,
            pierdeTurnoJ1 = false,
            pierdeTurnoJ2 = false,
            bloqueoPozo1 = false,
            bloqueoPozo2 = false;

    public Tablero tablero; // Tablero

    public Text

            textoJ1,
            textoJ2; // Textos de los jugadores

    private GestorDeEscenas gestorDeEscenas; // Gestor de escenas

    // Desplazamiento lateral de las fichas cuando colisionan
    private float lateralOffset = 0.5f;

    // Contador de turnos perdidos
    private int

            contadorTurnosPerdidosJ1 = 0,
            contadorTurnosPerdidosJ2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        turnoFicha1 = true;
        turnoFicha2 = false;
        gestorDeEscenas =
            GameObject.Find("GestorDeEscenas").GetComponent<GestorDeEscenas>();
        pierdeTurnoJ1 = EstadoDeEscena.ObtenerInstancia().pierdeTurnoJ1;
        pierdeTurnoJ2 = EstadoDeEscena.ObtenerInstancia().pierdeTurnoJ2;
        bloqueoPozo1 = EstadoDeEscena.ObtenerInstancia().bloqueoPozo1;
        bloqueoPozo2 = EstadoDeEscena.ObtenerInstancia().bloqueoPozo2;
    }

    // Update is called once per frame
    void Update()
    {
        // Si las fichas están en la misma posición, desplazarlas lateralmente
        if (
            ficha1.transform.position == ficha2.transform.position &&
            !ficha1.enMovimiento &&
            !ficha2.enMovimiento
        )
        {
            DesplazarFichas();
        }

        // Si el dado ha sido lanzado y ha caido, mover la ficha correspondiente
        if (dado.numeroDado != 0)
        {
            // Si es el turno de la ficha 1 moverla
            if (turnoFicha1)
            {
                // Si se pasa de la casilla 62, no se mueve
                if (ficha1.casillaActual + dado.numeroDado > 62)
                {
                    Debug.Log("No se puede mover (final)");
                    CambiarTurno();
                }
                else if (!pierdeTurnoJ1 && !bloqueoPozo1)
                    ficha1.Mover(ficha1.casillaActual + dado.numeroDado);
                else
                // Pierde turno
                {
                    Debug.Log("Pierde turno J1");
                    if (pierdeTurnoJ1) J1PierdeTurno();
                    EnviarAEstadoDeEscena();
                    CambiarTurno();
                }
            } // Si es el turno de la ficha 2 moverla
            else if (turnoFicha2)
            {
                if (ficha2.casillaActual + dado.numeroDado > 62)
                {
                    Debug.Log("No se puede mover");
                    CambiarTurno();
                }
                if (!pierdeTurnoJ2 && !bloqueoPozo2)
                    ficha2.Mover(ficha2.casillaActual + dado.numeroDado);
                else
                // Pierde turno
                {
                    Debug.Log("Pierde turno J1");
                    if (pierdeTurnoJ2) J2PierdeTurno();
                    EnviarAEstadoDeEscena();
                    CambiarTurno();
                }
            }
            dado.numeroDado = 0;
        }
        if (tocaMinijuego)
        {
            gestorDeEscenas.CargarPruebaMiniJuego();
            CambiarTurno();
        }
    }

    // Cambiar turno entre los jugadores
    public void CambiarTurno()
    {
        // Si es el turno del jugador 1 cambiar al jugador 2
        if (turnoFicha1)
        {
            // Cambiar el turno
            turnoFicha1 = false;
            turnoFicha2 = true;

            // Cambiar el color del dado y el borde del texto al del jugador 2
            dado.GetComponent<Renderer>().material.color = Color.blue;
            textoJ1.GetComponent<Outline>().enabled = false;
            textoJ2.GetComponent<Outline>().enabled = true;
        } // Si es el turno del jugador 2 cambia minijuego
        else if (turnoFicha2)
        {
            // Cambiar el turno
            turnoFicha2 = false;
            tocaMinijuego = true;
        } // Si es el turno del minijuego cambia al jugador 1
        else if (tocaMinijuego)
        {
            // Cambiar el turno
            turnoFicha1 = true;
            tocaMinijuego = false;

            // Cambiar el color del dado y el borde del texto al del jugador 1
            dado.GetComponent<Renderer>().material.color = Color.red;
            textoJ1.GetComponent<Outline>().enabled = true;
            textoJ2.GetComponent<Outline>().enabled = false;
        }
    }

    // Desplazar las fichas lateralmente si han caído en la misma casilla
    void DesplazarFichas()
    {
        int i = ficha1.casillaActual; // Indice de la casilla
        Vector3 offset;

        // Comprobamos si la casilla es horizontal o vertical
        if (tablero.CasillaHorizontal(i))
            offset = new Vector3(0, 0, lateralOffset);
        else
            offset = new Vector3(lateralOffset, 0, 0);

        // Desplazar las fichas
        ficha1.transform.position += offset;
        ficha2.transform.position -= offset;
    }

    // Método para comprobar si la ficha ha caído en una casilla especial
    public void ComprobarEvento(int indiceCasilla)
    {
        Casilla.TipoCasilla t =
            tablero.ObtenerCasillaPorIndice(indiceCasilla).tipoCasilla_;
        switch (t)
        {
            case Casilla.TipoCasilla.fin:
                EventoFin();
                break;
            case Casilla.TipoCasilla.Oca:
                if (!ocaPrevia)
                    // Esto es para evitar que se cambie de turno si se ha avanzado a la siguiente oca
                    EventoOca(indiceCasilla);
                else
                    ocaPrevia = false;
                break;
            case Casilla.TipoCasilla.Puente:
                if (!puentePrevio)
                    EventoPuente(indiceCasilla);
                else
                    puentePrevio = false;
                break;
            case Casilla.TipoCasilla.Dado:
                if (!dadoPrevio)
                    EventoDado(indiceCasilla);
                else
                    dadoPrevio = false;
                break;
            case Casilla.TipoCasilla.Posada:
                EventoPosada();
                break;
            case Casilla.TipoCasilla.Laberinto:
                EventoLaberinto();
                break;
            case Casilla.TipoCasilla.Carcel:
                EventoCarcel();
                break;
            case Casilla.TipoCasilla.Muerte:
                EventoMuerte();
                break;
            case Casilla.TipoCasilla.Pozo:
                EventoPozo();
                break;
            default:
                Debug.Log("No hay evento");
                CambiarTurno();
                break;
        }
    }

    private void J1PierdeTurno()
    {
        Debug.Log("Pierde turno J1");
        contadorTurnosPerdidosJ1--;
        Debug.Log(contadorTurnosPerdidosJ1);
        if (contadorTurnosPerdidosJ1 == 0)
        {
            pierdeTurnoJ1 = false;
        }
    }

    private void J2PierdeTurno()
    {
        Debug.Log("Pierde turno J2");
        contadorTurnosPerdidosJ2--;
        if (contadorTurnosPerdidosJ2 == 0)
        {
            pierdeTurnoJ2 = false;
        }
    }

    private void EventoFin()
    {
        if (turnoFicha1)
        {
            gestorDeEscenas.CargarEscenaWin1();
        }
        else if (turnoFicha2)
        {
            gestorDeEscenas.CargarEscenaWin2();
        }
    }

    private void EventoOca(int posicion)
    {
        Debug.Log("De Oca en Oca y tiro por que me toca");

        // Avanzar a la siguiente oca
        int i = tablero.ObtenerSiguienteOca(posicion);
        if (turnoFicha1)
        {
            ficha1.Mover (i);
        }
        else if (turnoFicha2)
        {
            ficha2.Mover (i);
        }
        ocaPrevia = true;
    }

    private void EventoPuente(int posicion)
    {
        Debug.Log("De puente a puente y tiro por que me lleva la corriente");

        // Si es el primer puente, avanza al segundo. Si no, retrocede al primero
        if (posicion == 5)
        {
            if (turnoFicha1)
            {
                ficha1.Mover(11);
            }
            else if (turnoFicha2)
            {
                ficha2.Mover(11);
            }
        }
        else if (posicion == 11)
        {
            if (turnoFicha1)
            {
                ficha1.MoverPatras(5);
            }
            else if (turnoFicha2)
            {
                ficha2.MoverPatras(5);
            }
        }
        puentePrevio = true;
    }

    private void EventoDado(int posicion)
    {
        Debug.Log("De dado a dado y tiro por que me ha tocado");

        // Si es el primer puente, avanza al segundo. Si no, retrocede al primero
        if (posicion == 25)
        {
            if (turnoFicha1)
            {
                ficha1.Mover(52);
            }
            else if (turnoFicha2)
            {
                ficha2.Mover(52);
            }
        }
        else if (posicion == 52)
        {
            if (turnoFicha1)
            {
                ficha1.MoverPatras(25);
            }
            else if (turnoFicha2)
            {
                ficha2.MoverPatras(25);
            }
        }
        dadoPrevio = true;
    }

    private void EventoPosada()
    {
        Debug.Log("Posada");
        if (turnoFicha1)
        {
            contadorTurnosPerdidosJ1 = 1;
            pierdeTurnoJ1 = true;
        }
        else if (turnoFicha2)
        {
            contadorTurnosPerdidosJ2 = 1;
            pierdeTurnoJ2 = true;
        }
        EnviarAEstadoDeEscena();

        // Cambiar el turno
        CambiarTurno();
    }

    private void EventoLaberinto()
    {
        Debug.Log("Laberinto");
        if (turnoFicha1)
        {
            ficha1.MoverPatras(30);
        }
        else if (turnoFicha2)
        {
            ficha2.MoverPatras(30);
        }

        // Cambiar el turno
        CambiarTurno();
    }

    private void EventoCarcel()
    {
        Debug.Log("Carcel");
        if (turnoFicha1)
        {
            contadorTurnosPerdidosJ1 = 2;
            pierdeTurnoJ1 = true;
        }
        else if (turnoFicha2)
        {
            contadorTurnosPerdidosJ2 = 2;
            pierdeTurnoJ2 = true;
        }
        EnviarAEstadoDeEscena();

        // Cambiar el turno
        CambiarTurno();
    }

    private void EventoMuerte()
    {
        Debug.Log("Muerte");
        if (turnoFicha1)
        {
            ficha1.MoverPatras(0);
        }
        else if (turnoFicha2)
        {
            ficha2.MoverPatras(0);
        }

        // Cambiar el turno
        CambiarTurno();
    }

    private void EventoPozo()
    {
        Debug.Log("Pozo");
        if (turnoFicha1)
        {
            bloqueoPozo1 = true;

            // Si la otra ficha lo ha superado, desbloquear
            if (ficha2.casillaActual >= ficha1.casillaActual)
            {
                bloqueoPozo1 = false;
            }
        }
        else if (turnoFicha2)
        {
            bloqueoPozo2 = true;

            // Si la otra ficha lo ha superado, desbloquear
            if (ficha1.casillaActual >= ficha2.casillaActual)
            {
                bloqueoPozo2 = false;
            }
        }

        // Cambiar el turno
        CambiarTurno();
    }

    // Enviar la casilla actual al EstadoDeEscena
    private void EnviarAEstadoDeEscena()
    {
        if (turnoFicha1)
        {
            EstadoDeEscena.ObtenerInstancia().pierdeTurnoJ1 = pierdeTurnoJ1;
            EstadoDeEscena.ObtenerInstancia().bloqueoPozo1 = bloqueoPozo1;
        }
        else if (turnoFicha2)
        {
            EstadoDeEscena.ObtenerInstancia().pierdeTurnoJ2 = pierdeTurnoJ2;
            EstadoDeEscena.ObtenerInstancia().bloqueoPozo2 = bloqueoPozo2;
        }
    }
}
