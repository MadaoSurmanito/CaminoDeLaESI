using UnityEngine;

// Clase para manejar el tablero de juego de la Oca
public class Tablero : MonoBehaviour
{
    // Array de casillas para representar el tablero
    public Casilla[] casillas;

    // Start is called before the first frame update
    void Start()
    {
        // Crear el tablero de juego
        CrearTablero();
    }

    // Método para crear el tablero de juego
    private void CrearTablero()
    {
        int auxX = -1;
        int auxY = 0;

        // Recorrer el array de casillas
        for (int i = 0; i < casillas.Length; i++)
        {
            casillas[i].numero_ = i;
            if ((i <= 8) || (i > 36 && i <= 44) || (i > 60 && i <= 62))
            {
                auxX++;
            }
            else if ((i > 8 && i <= 18) || (i > 44 && i <= 50))
            {
                auxY++;
            }
            else if ((i > 18 && i <= 28) || (i > 50 && i <= 56))
            {
                auxX--;
            }
            else if ((i > 28 && i <= 36) || (i > 56 && i <= 60))
            {
                auxY--;
            }
            else
            {
                Debug.LogError("Error al instanciar casilla " + i);
            }
            instanciarCasilla (i, auxX, auxY);
        }
    }

    void instanciarCasilla(int indice, int x, int z)
    {
        // Instanciar la casilla
        GameObject c =
            Instantiate(casillas[indice].prefabCasilla_,
            new Vector3(x * 3, 0, z * 3),
            Quaternion.identity);
        casillas[indice].posX = x;
        casillas[indice].posZ = z;
        cambiarColorYTipo (c, indice);
    }

    void cambiarColorYTipo(GameObject c, int i)
    {
        c.name = "Casilla " + i;

        // Si 0 o 62: Inicio y fin
        if (i == 0 || i == 62)
        {
            c.GetComponent<Renderer>().material.color = Color.green;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.iniFin;
        } // Si 4, 8, 13, 17, 22, 26, 31, 35, 40, 44, 49, 53, 58: Oca
        else if (
            i == 4 ||
            i == 8 ||
            i == 13 ||
            i == 17 ||
            i == 22 ||
            i == 26 ||
            i == 31 ||
            i == 35 ||
            i == 40 ||
            i == 44 ||
            i == 49 ||
            i == 53 ||
            i == 58
        )
        {
            c.GetComponent<Renderer>().material.color = Color.cyan;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Oca;
        } // Si 5 y 11: Puente
        else if (i == 5 || i == 11)
        {
            c.GetComponent<Renderer>().material.color = Color.yellow;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Puente;
        } // Si 18: Posada
        else if (i == 18)
        {
            c.GetComponent<Renderer>().material.color = Color.magenta;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Posada;
        } // Si 25 y 52 : Dados
        else if (i == 25 || i == 52)
        {
            c.GetComponent<Renderer>().material.color = Color.grey;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Dados;
        } // Si 30: Pozo
        else if (i == 30)
        {
            c.GetComponent<Renderer>().material.color = Color.black;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Pozo;
        } // Si 57: Muerte
        else if (i == 57)
        {
            c.GetComponent<Renderer>().material.color = Color.red;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Muerte;
        } // Si 41 Laberinto
        else if (i == 41)
        {
            c.GetComponent<Renderer>().material.color = Color.blue;
            casillas[i].tipoCasilla_ = Casilla.TipoCasilla.Laberinto;
        } // Si 55 Carcel
        else if (i == 55)
        {
            c.GetComponent<Renderer>().material.color = Color.white;
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
}
