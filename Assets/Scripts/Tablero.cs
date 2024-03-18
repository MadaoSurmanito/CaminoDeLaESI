using UnityEngine;

// Clase para manejar el tablero de juego de la Oca
public class Tablero : MonoBehaviour
{
    // Array de casillas para representar el tablero
    public Casilla[] casillas;

    void Start()
    {
        // Crear el tablero de juego
        CrearTablero();
    }

    // Método para crear el tablero de juego
    private void CrearTablero()
    {
        int auxX = -1; // Coordenada X, empieza en -1 para que la primera casilla se instancie en 0
        int auxY = 0; // Coordenada Y

        // Recorrer el array de casillas
        for (int i = 0; i < casillas.Length; i++)
        {
            // Asignar el número de la casilla
            casillas[i].numero_ = i;
            // Esto instancia las casillas en el tablero en la posición correspondiente
            // Mirar el excel para entenderlo
            if (TramoDeIzquierdaADerecha(i))
                auxX++;
            else if (TramoDeArribaAbajo(i))
                auxY++;
            else if (TramoDeDerechaIzquierda(i))
                auxX--;
            else if (TramoDeAbajoArriba(i))
                auxY--;
            else
                Debug.LogError("Error al instanciar casilla " + i);
            instanciarCasilla (i, auxX, auxY);
        }
    }

    void instanciarCasilla(int i, int x, int z)
    {
        // Instanciar la casilla
        GameObject c =
            Instantiate(casillas[i].prefabCasilla_,
            new Vector3(x * 3, 0, z * 3),
            Quaternion.identity);
        
        casillas[i].posX = x;
        casillas[i].posZ = z;
        c.name = "Casilla " + i;

        // Si 0 o 62: Inicio y fin
        if (i == 0 || i == 62)
        {
            c.GetComponent<Renderer>().material.color = Color.green;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.iniFin;
        }
        // Si 4, 8, 13, 17, 22, 26, 31, 35, 40, 44, 49, 53, 58: Oca
        else if (
            // Esta guapisimo C# a que si?
            i is 4 or 8 or 13 or 17 or 22 or 26 or 31 or 35 or 40 or 44 or 49 or 53 or 58
        )
        {
            c.GetComponent<Renderer>().material.color = new Color(1f, 0.6f, 0);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Oca;
        } // Si 5 y 11: Puente
        else if (i == 5 || i == 11)
        {
            c.GetComponent<Renderer>().material.color = new Color(0.0275f, 0.5255f, 0.9098f);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Puente;
        } // Si 18: Posada
        else if (i == 18)
        {
            c.GetComponent<Renderer>().material.color = new Color(1f, 1f, 0);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Posada;
        } // Si 25 y 52 : Dados
        else if (i == 25 || i == 52)
        {
            c.GetComponent<Renderer>().material.color = new Color(1f, 0, 1f);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Dados;
        } // Si 30: Pozo
        else if (i == 30)
        {
            c.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Pozo;
        } // Si 57: Muerte
        else if (i == 57)
        {
            c.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Muerte;
        } // Si 41 Laberinto
        else if (i == 41)
        {
            c.GetComponent<Renderer>().material.color = new Color(1f, 0, 0);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Laberinto;
        } // Si 55 Carcel
        else if (i == 55)
        {
            c.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f);
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Carcel;
        }
        else
        // De lo contrario, casilla normal
        {
            c.GetComponent<Renderer>().material.color = Color.white;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Positiva;
        }
    }

    // Método para obtener una casilla por su índice
    public Casilla ObtenerCasillaPorIndice(int indice)
    {
        if (indice >= 0 && indice < casillas.Length)
        {
            return casillas[indice];
        }
        else
        {
            return null;
        }
    }

    // Método que indica si una casilla es horizontal
    // Una casilla "Horizontal" representa que se recorre de izquierda a derecha
    // Una casilla "Vertical" representa que se recorre de arriba a abajo
    public bool CasillaHorizontal(int i)
    {
        return (i <= 8) ||
        (i > 36 && i <= 44) ||
        (i > 60 && i <= 62) ||
        (i > 18 && i <= 28) ||
        (i > 50 && i <= 56);

    }

    public bool CasillaVertical(int i)
    {
        return !CasillaHorizontal(i);
    }

    public bool TramoDeIzquierdaADerecha(int i)
    {
        return (i <= 8) || (i > 36 && i <= 44) || (i > 60 && i <= 62);
    }

    public bool TramoDeArribaAbajo(int i)
    {
        return (i > 8 && i <= 18) || (i > 44 && i <= 50);
    }

    public bool TramoDeDerechaIzquierda(int i)
    {
        return (i > 18 && i <= 28) || (i > 50 && i <= 56);
    }

    public bool TramoDeAbajoArriba(int i)
    {
        return (i > 28 && i <= 36) || (i > 56 && i <= 60);
    }
}
