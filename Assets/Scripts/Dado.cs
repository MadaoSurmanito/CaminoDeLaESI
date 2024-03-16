using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dado : MonoBehaviour
{
    public Text texto;

    public Cara[] caras;

    public int numeroDado;

    // Start is called before the first frame update
    void Start()
    {
        NumeroDado();
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = "Número: " + numeroDado;
    }

    void NumeroDado()
    {
        for (int i = 0; i < caras.Length; i++)
        {
            if (caras[i].TocaSuelo)
            {
                numeroDado = 7 - caras[i].numeroCara;
            }
        }
        Invoke("NumeroDado", 0.5f);
    }

    public void LanzarDado()
    {
        float fuerzaInicial = Random.Range(1, 6);
        GetComponent<Rigidbody>().isKinematic = false;
        Quaternion rotacionInicial =
            new Quaternion(Random.Range(-10, 10),
                Random.Range(-10, 10),
                Random.Range(-10, 10),
                1);
        GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaInicial, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(rotacionInicial.eulerAngles);
    }
}
