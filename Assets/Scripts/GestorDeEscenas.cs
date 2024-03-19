using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorDeEscenas : MonoBehaviour
{
    // Método para cargar una nueva escena y conservar el estado anterior
    public void CargarEscena(string nombreEscena)
    {
        // Obtener la instancia del EstadoDeEscena
        EstadoDeEscena estadoDeEscena = EstadoDeEscena.ObtenerInstancia();

        // Si hay una instancia de EstadoDeEscena
        if (estadoDeEscena != null)
        {
            // Cargar la nueva escena
            SceneManager.LoadScene (nombreEscena);
        }
        else
        {
            Debug
                .LogWarning("No se encontró una instancia de EstadoDeEscena. No se puede cargar la nueva escena.");
        }
    }

    // Método para cargar la escena "pruebaMiniJuego" y conservar el estado
    public void CargarPruebaMiniJuego()
    {
        CargarEscena("pruebaMiniJuego");
    }

    // Método para cargar la escena escenaOCA
    public void CargarEscenaOCA()
    {
        CargarEscena("escenaOCA");
    }

    // Método para cargar la escena escena win 1
    public void CargarEscenaWin1()
    {
        CargarEscena("escenaWin1");
    }

    // Método para cargar la escena escena win 2
    public void CargarEscenaWin2()
    {
        CargarEscena("escenaWin2");
    }
}
