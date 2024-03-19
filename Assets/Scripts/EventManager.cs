using UnityEngine;

// Clase para manejar los eventos del juego
public class EventManager : MonoBehaviour
{
    // Método para ejecutar el evento de avanzar a la siguiente oca
    public void AvanzarSiguienteOca(int posicion)
    {
        Debug.Log("Avanzar a la siguiente oca desde la posición " + posicion);
    }
}