using UnityEngine;
using UnityEngine.UI;

public class GestorDeTurnos : MonoBehaviour
{
    public Ficha

            ficha1,
            ficha2; // Fichas de los jugadores

    public Dado dado; // Dado

    public bool

            turnoFicha1,
            turnoFicha2,
            tocaMinijuego; // Turnos de los jugadores

    bool deOcaEnOcaPrevio = false;

    public Tablero tablero; // Tablero

    public Text

            textoJ1,
            textoJ2; // Textos de los jugadores

    // Desplazamiento lateral de las fichas cuando colisionan
    private float lateralOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        turnoFicha1 = true;
        turnoFicha2 = false;
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
                ficha1.Mover(ficha1.casillaActual + dado.numeroDado);
            } // Si es el turno de la ficha 2 moverla
            else if (turnoFicha2)
            {
                ficha2.Mover(ficha2.casillaActual + dado.numeroDado);
            }
            dado.numeroDado = 0;
        }
        if (tocaMinijuego)
        {
            Debug.Log("Toca minijuego");
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
            tocaMinijuego = false;

            // Cambiar el color  y el borde del texto al del jugador 2
            dado.GetComponent<Renderer>().material.color = Color.blue;
            textoJ1.GetComponent<Outline>().enabled = false;
            textoJ2.GetComponent<Outline>().enabled = true;
        } // Si es el turno del jugador 2 cambia minijuego
        else if (turnoFicha2)
        {
            // Cambiar el turno
            turnoFicha1 = false;
            turnoFicha2 = false;
            tocaMinijuego = true;
        }
        // Si es el turno del minijuego cambia al jugador 1
        else if (tocaMinijuego)
        {
            // Cambiar el turno
            turnoFicha1 = true;
            turnoFicha2 = false;
            tocaMinijuego = false;

            // Cambiar el color  y el borde del texto al del jugador 1
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
            case Casilla.TipoCasilla.Oca:
                // Esto es para evitar que se cambie de turno si se ha avanzado a la siguiente oca
                if (!deOcaEnOcaPrevio)
                    EventoOca(indiceCasilla);
                else
                    deOcaEnOcaPrevio = false;
                break;
            case Casilla.TipoCasilla.Puente:
                Debug.Log("De puente a puente y tiro por que me lleva la corriente");
                CambiarTurno();
                break;
            default:
                CambiarTurno();
                break;
        }
    }

    // Método para ejecutar el evento de avanzar a la siguiente oca
    public void EventoOca(int posicion)
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
        deOcaEnOcaPrevio = true;
    }
}