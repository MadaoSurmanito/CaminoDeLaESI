using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorDeTurnos : MonoBehaviour
{
    public Ficha ficha1;

    public Ficha ficha2;

    public Dado dado;

    public bool turnoFicha1;

    public bool turnoFicha2;

    public Tablero tablero;

    // Start is called before the first frame update
    void Start()
    {
        turnoFicha1 = true;
        turnoFicha2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dado.numeroDado != 0)
        {
            if (turnoFicha1)
            {

                ficha1
                    .Mover(ficha1.casillaActual + dado.numeroDado);
                    // Comprobar evento
                turnoFicha1 = false;
                turnoFicha2 = true;

                // Cambiar color del material a rojo
                dado.GetComponent<Renderer>().material.color = Color.blue;
            }
            else if (turnoFicha2)
            {
                ficha2
                    .Mover(ficha2.casillaActual + dado.numeroDado);
                turnoFicha2 = false;
                turnoFicha1 = true;

                // Cambiar material del dado a azul
                dado.GetComponent<Renderer>().material.color = Color.red;
            }
            dado.numeroDado = 0;
        }
    }
}
