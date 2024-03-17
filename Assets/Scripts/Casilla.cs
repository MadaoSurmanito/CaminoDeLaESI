using UnityEngine;

// Estructura para representar una casilla en el tablero
[System.Serializable]
public class Casilla
{
    // Propiedades de la casilla
    public int numero_;

    public int posX, posZ;

    public GameObject prefabCasilla_;

    public enum TipoCasilla
    {
        iniFin,
        Positiva,
        Negativa,
        Oca,
        Puente,
        Dados,
        Posada,
        Pozo,
        Muerte,
        Laberinto,
        Carcel,
        Tienda
    }

    public TipoCasilla tipoCasilla_;

    public Vector3 ObtenerPosicion()
    {
        return new Vector3(posX * 3, 1, posZ * 3);
    }
}
