using UnityEngine;

public class NPCproximidad : MonoBehaviour
{
    public Transform player;
    public float distanciaActivacion = 3f;

    public Animator animator;

    [Header("Animación")]
    public string triggerNombre = "Saludar";

    private bool yaActivado = false;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (player == null || animator == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        // 👉 ENTRA en rango → dispara animación
        if (distancia <= distanciaActivacion && !yaActivado)
        {
            animator.SetTrigger(triggerNombre);
            yaActivado = true;
        }

        // 👉 SALE del rango → permite que se vuelva a activar
        if (distancia > distanciaActivacion)
        {
            yaActivado = false;
        }
    }
}