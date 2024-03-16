using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour
{
    public Dado dado;

    int numeroMovimientoActual;

    int casillaActual;

    // Start is called before the first frame update
    void Start()
    {
        casillaActual = 1;
        numeroMovimientoActual = dado.numeroDado;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Moverse hasta llegar a la posición destino
    public void Mover(Vector3 destino)
    {
        StartCoroutine(MoverCoroutine(destino));
        numeroMovimientoActual = dado.numeroDado;
        casillaActual += dado.numeroDado;
    }

    // Corrutina para moverse
    private IEnumerator MoverCoroutine(Vector3 destino)
    {
        while (Vector3.Distance(transform.position, destino) > 0.1f)
        {
            transform.position =
                Vector3
                    .MoveTowards(transform.position,
                    destino,
                    5f * Time.deltaTime);
            yield return null;
        }
        transform.position = destino;
    }
}
