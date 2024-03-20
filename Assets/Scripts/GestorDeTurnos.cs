using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GestorDeTurnos : MonoBehaviour
{
    // Variables de los jugadores
    public Ficha ficha1; // Ficha J1

    public Ficha ficha2; // Ficha J2

    public Dado dado; // Dado

    public bool
        // Turnos de los jugadores y minijuego

            turnoFicha1,
            turnoFicha2,
            tocaMinijuego;

    private bool
        // Variables para controlar los eventos de las casillas especiales
        // Como siempre se cambia de turno después de un evento,
        // se necesita un booleano para evitar que se cambie de turno dos veces

            ocaPrevia = false,
            puentePrevio = false,
            dadoPrevio = false;

    public Tablero tablero; // Tablero

    public Text

            textoJ1,
            textoJ2; // Textos de los jugadores

    private GestorDeEscenas gestorDeEscenas; // Gestor de escenas

    // Desplazamiento lateral de las fichas cuando colisionan
    private float lateralOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        turnoFicha1 = true;
        turnoFicha2 = false;
        gestorDeEscenas = GameObject.Find("GestorDeEscenas").GetComponent<GestorDeEscenas>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si las fichas están en la misma posición, desplazarlas lateralmente
        if ((ficha1.transform.position == ficha2.transform.position) && !ficha1.enMovimiento && !ficha2.enMovimiento)
            DesplazarFichas();

        // Si el dado ha sido lanzado y ha caido, mover la ficha correspondiente
        if (dado.numeroDado != 0)
        {
            // Si es el turno de la ficha 1 moverla
            if (turnoFicha1)
                Turno(ficha1); // Si es el turno de la ficha 2 moverla
            else if (turnoFicha2) Turno(ficha2);
            dado.numeroDado = 0;
        }
        if (tocaMinijuego)
        {
            // ESperar a que j1 y j2 terminen de moverse
            if (!ficha1.enMovimiento && !ficha2.enMovimiento)
            {
                gestorDeEscenas.CargarPruebaMiniJuego();
                CambiarTurno();
            }
        }
    }

    // Método para gestionar el turno de las fichas
    private void Turno(Ficha ficha)
    {
        if (ficha.casillaActual + dado.numeroDado > 62)
        {
            Debug.Log("No se puede mover");
            CambiarTurno();
        }
        if (!ficha.pierdeTurno && !ficha.bloqueoPozo)
            ficha.Mover(ficha.casillaActual + dado.numeroDado);
        else
        // Pierde turno
        {
            if (ficha.pierdeTurno) PierdeTurno(ficha);
            EnviarAEstadoDeEscena (ficha);
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
    public void ComprobarEvento(Ficha ficha)
    {
        Casilla.TipoCasilla t = tablero.ObtenerCasillaPorIndice(ficha.casillaActual).tipoCasilla_;
        switch (t)
        {
            case Casilla.TipoCasilla.fin:
                EventoFin (ficha);
                break;
            case Casilla.TipoCasilla.Oca:
                if (!ocaPrevia)
                    // Esto es para evitar que se cambie de turno si se ha avanzado a la siguiente oca
                    EventoOca(ficha);
                else
                    ocaPrevia = false;
                break;
            case Casilla.TipoCasilla.Puente:
                if (!puentePrevio)
                    EventoPuente(ficha);
                else
                    puentePrevio = false;
                break;
            case Casilla.TipoCasilla.Dado:
                if (!dadoPrevio)
                    EventoDado(ficha);
                else
                    dadoPrevio = false;
                break;
            case Casilla.TipoCasilla.Posada:
                EventoPosada (ficha);
                break;
            case Casilla.TipoCasilla.Laberinto:
                EventoLaberinto (ficha);
                break;
            case Casilla.TipoCasilla.Carcel:
                EventoCarcel (ficha);
                break;
            case Casilla.TipoCasilla.Muerte:
                EventoMuerte (ficha);
                break;
            case Casilla.TipoCasilla.Pozo:
                EventoPozo (ficha);
                break;
            default:
                Debug.Log("No hay evento");
                CambiarTurno();
                break;
        }
    }

    private void PierdeTurno(Ficha ficha)
    {
        Debug.Log("Pierde turno" + ficha.name);
        ficha.contadorTurnosPerdidos--;
        Debug.Log(ficha.contadorTurnosPerdidos);
        if (ficha.contadorTurnosPerdidos == 0)
        {
            ficha.pierdeTurno = false;
        }
    }

    private void EventoFin(Ficha ficha)
    {
        if (ficha == ficha1)
        {
            gestorDeEscenas.CargarEscenaWin1();
        }
        else if (ficha == ficha2)
        {
            gestorDeEscenas.CargarEscenaWin2();
        }
    }

    private void EventoOca(Ficha ficha)
    {
        Debug.Log("De Oca en Oca y tiro por que me toca");

        // Avanzar a la siguiente oca
        int i = tablero.ObtenerSiguienteOca(ficha.casillaActual);
        ficha.Mover (i);
        ocaPrevia = true;
    }

    private void EventoPuente(Ficha ficha)
    {
        Debug.Log("De puente a puente y tiro por que me lleva la corriente");

        // Si es el primer puente, avanza al segundo. Si no, retrocede al primero
        if (ficha.casillaActual == 5)
            ficha.Mover(11);
        else if (ficha.casillaActual == 11) ficha.MoverPatras(5);
        puentePrevio = true;
    }

    private void EventoDado(Ficha ficha)
    {
        Debug.Log("De dado a dado y tiro por que me ha tocado");

        // Si es el primer puente, avanza al segundo. Si no, retrocede al primero
        if (ficha.casillaActual == 25)
            ficha.Mover(52);
        else if (ficha.casillaActual == 52) ficha.MoverPatras(25);

        dadoPrevio = true;
    }

    private void EventoPosada(Ficha ficha)
    {
        Debug.Log("Posada");

        ficha.contadorTurnosPerdidos = 1;
        ficha.pierdeTurno = true;

        EnviarAEstadoDeEscena (ficha);

        // Cambiar el turno
        CambiarTurno();
    }

    private void EventoLaberinto(Ficha ficha)
    {
        Debug.Log("Laberinto");
        ficha.MoverPatras(30);
        CambiarTurno();
    }

    private void EventoCarcel(Ficha ficha)
    {
        Debug.Log("Carcel");
        ficha.contadorTurnosPerdidos = 2;
        ficha.pierdeTurno = true;
        EnviarAEstadoDeEscena (ficha);

        // Cambiar el turno
        CambiarTurno();
    }

    private void EventoMuerte(Ficha ficha)
    {
        Debug.Log("Muerte");
        ficha.MoverPatras(0);
        CambiarTurno();
    }

    private void EventoPozo(Ficha ficha)
    {
        Debug.Log("Pozo");
        Ficha aux;
        if (ficha == ficha1)
            aux = ficha2;
        else
            aux = ficha1;

        // Si la posición de aux es menor que ficha, ficha.blockPozo = true
        if (aux.casillaActual < ficha.casillaActual)
        {
            ficha.bloqueoPozo = true;
        }
        else
            ficha.bloqueoPozo = false;

        // Cambiar el turno
        CambiarTurno();
    }

    // Enviar la casilla actual al EstadoDeEscena
    private void EnviarAEstadoDeEscena(Ficha ficha)
    {
        ficha.EnviarAEstadoDeEscena();
    }
}
