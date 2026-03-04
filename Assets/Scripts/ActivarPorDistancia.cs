using UnityEngine;

public class ActivarPorDistancia : MonoBehaviour
{
    public Transform jugador;          // Main Camera
    public float distanciaActivacion = 3f;

    public Renderer pantalla;          // Renderer de la pantalla
    public Material apagado;           // Material negro
    public Material encendido;         // Material con luz

    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaActivacion)
            pantalla.material = encendido;
        else
            pantalla.material = apagado;
    }
}