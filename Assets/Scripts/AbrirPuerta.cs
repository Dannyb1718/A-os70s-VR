using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public float anguloApertura = 90f;
    public float velocidad = 2f;
    private bool abrir = false;
    private Quaternion rotacionInicial;
    private Quaternion rotacionFinal;

    void Start()
    {
        rotacionInicial = transform.rotation;
        rotacionFinal = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));
    }

    void Update()
    {
        if (abrir)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionFinal, Time.deltaTime * velocidad);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            abrir = true;
        }
    }
}