using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GestorDeEscenas : MonoBehaviour
{
    // cargar escena tablero
    // dontDestroyOnLoad
    public void CargarEscenaTablero()
    {
        SceneManager.LoadScene("EscenaOCA");
    }

    void Awake()
    {
        // Este objeto no se destruirá al cargar una nueva escena
        DontDestroyOnLoad (gameObject);
    }

    // cargar escena prubeaMinijuego
    public void CargarEscenaPruebaMinijuego()
    {
        SceneManager.LoadScene("PruebaMinijuego");
    }
}
