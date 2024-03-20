using UnityEngine;

public class EstadoDeEscena : MonoBehaviour
{
    private static EstadoDeEscena instance;

    // Puedes agregar aquí cualquier dato que desees conservar entre escenas
    public struct ValorJugador
    {
        public int casilla;

        public bool pierdeTurno;

        public bool bloqueoPozo;

        public int contadorTurnosPerdidos;

        public int creditos;
    }

    public ValorJugador[] valoresJugadores = new ValorJugador[2];

    private void Awake()
    {
        // Asegurarse de que solo haya una instancia de este script en toda la escena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Método para obtener la instancia del EstadoDeEscena
    public static EstadoDeEscena ObtenerInstancia()
    {
        return instance;
    }
}
