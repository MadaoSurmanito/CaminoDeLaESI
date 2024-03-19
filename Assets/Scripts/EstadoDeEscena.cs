using UnityEngine;

public class EstadoDeEscena : MonoBehaviour
{
    private static EstadoDeEscena instance;

    // Puedes agregar aquí cualquier dato que desees conservar entre escenas
    public int casillaJ1;
    public int casillaJ2;
    public bool pierdeTurnoJ1;
    public bool pierdeTurnoJ2;
    public bool bloqueoPozo1;
    public bool bloqueoPozo2;

    private void Awake()
    {
        // Asegurarse de que solo haya una instancia de este script en toda la escena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Método para obtener la instancia del EstadoDeEscena
    public static EstadoDeEscena ObtenerInstancia()
    {
        return instance;
    }
}