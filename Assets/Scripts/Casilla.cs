using UnityEngine;

// Estructura para representar una casilla en el tablero
[System.Serializable]
public class Casilla
{
    // Esta clase representa una casilla del tablero de la Oca
    // Cada casilla tiene un número, una posición y un tipo
    // El número es el índice de la casilla en el array de casillas
    // La posición es la coordenada (x, z) del centro de la casilla
    // El prefabCasilla es el prefab de la casilla que se instanciará en el tablero
    public int numero_; // Numero de la casilla

    public int

            posX,
            posZ; // Coordenadas del centro de la casilla

    public GameObject prefabCasilla_; // Prefab de la casilla

    public enum TipoCasilla
    {
        inicio,
        fin,
        normal,
        Oca,
        Puente,
        Dado,
        Posada,
        Pozo,
        Muerte,
        Laberinto,
        Carcel,
        Tienda
    }

    public TipoCasilla tipoCasilla_;

    // Método para obtener la posición de la casilla
    public Vector3 ObtenerPosicion()
    {
        return new Vector3(posX * 3, 1, posZ * 3);
    }
}
