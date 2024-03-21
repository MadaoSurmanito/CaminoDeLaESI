using UnityEngine;

// Clase para manejar el tablero de juego de la Oca
public class Tablero : MonoBehaviour
{
    // Array de casillas para representar el tablero
    public Casilla[] casillas;
    // Array de prefabs de casillas
    public GameObject[] prefabsCasillas;

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
            InstanciarCasilla (i, auxX, auxY);
        }
    }

    void InstanciarCasilla(int i, int x, int z)
    {
        // Instanciar la casilla
        casillas[i].posX = x;
        casillas[i].posZ = z;
        // Si 0: inicio
        if (i == 0)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.inicio);
            c.GetComponent<Renderer>().material.color = Color.green;
        }
        // Si 63: fin
        else if (i == 62)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.fin);
            c.GetComponent<Renderer>().material.color = Color.green;
        }
        // Si 4, 8, 13, 17, 22, 26, 31, 35, 40, 44, 49, 53, 58: Oca
        else if (i is 4 or 8 or 13 or 17 or 22 or 26 or 31 or 35 or 40 or 44 or 49 or 53 or 58)
            _ = InstanciarCasillaEspecifica(i, x, z, 1, Casilla.TipoCasilla.Oca);
        // Si 5 y 11: Puente
        else if (i == 5 || i == 11)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.Puente);
            c.GetComponent<Renderer>().material.color = new Color(0.0275f, 0.5255f, 0.9098f);

        } // Si 18: Posada
        else if (i == 18)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.Posada);
            c.GetComponent<Renderer>().material.color = new Color(1f, 1f, 0);
        } // Si 25 y 52 : Dados
        else if (i == 25 || i == 52)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.Dado);
            c.GetComponent<Renderer>().material.color = new Color(1f, 0, 1f);
        } // Si 30: Pozo
        else if (i == 30)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.Pozo);
            c.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);
        } // Si 57: Muerte
        else if (i == 57)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.Muerte);
            c.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        } // Si 41 Laberinto
        else if (i == 41)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.Laberinto);
            c.GetComponent<Renderer>().material.color = new Color(1f, 0, 0);
        } // Si 55 Carcel
        else if (i == 55)
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.Carcel);
            c.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f);
        }
        else
        // De lo contrario, casilla normal
        {
            GameObject c = InstanciarCasillaEspecifica(i, x, z, 0, Casilla.TipoCasilla.normalGood);
            c.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    GameObject InstanciarCasillaEspecifica(int i, int x, int z, int indicePrefab, Casilla.TipoCasilla tipoCasilla)
    {
        GameObject c =
        Instantiate(prefabsCasillas[indicePrefab],
        new Vector3(x * 3, 0, z * 3),
        Quaternion.identity);
        casillas[i].tipoCasilla_ = tipoCasilla;
        c.name = "Casilla " + i;
        return c;
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

    // Funcion que devuelve el indice de la siguiente oca en base a la posicion recibida
    public int ObtenerSiguienteOca(int posicion)
    {
        for (int i = posicion + 1; i < casillas.Length; i++)
        {
            if (casillas[i].tipoCasilla_ == Casilla.TipoCasilla.Oca)
            {
                return i;
            }
        }
        return -1;
    }
}
