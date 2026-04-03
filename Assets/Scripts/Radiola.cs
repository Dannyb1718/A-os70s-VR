using UnityEngine;

public class RadiolaController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicaSource;   // Música principal (3D)
    public AudioSource sfxSource;      // Sonido de clic
    public AudioClip clickSound;       // Sonido al encender

    [Header("Canciones")]
    public AudioClip[] canciones;
    private int indiceActual = 0;

    private bool encendida = false;

    void Start()
    {
        Debug.Log("Radiola lista");

        if (musicaSource != null)
            musicaSource.Stop();

        if (sfxSource != null)
            sfxSource.Stop();
    }

    void Update()
    {
        // 👉 Clic derecho para cambiar canción (solo si está encendida)
        if (encendida && Input.GetMouseButtonDown(1))
        {
            CambiarCancion();
        }
    }

    // 👉 CLICK CON MOUSE (pruebas PC)
    void OnMouseDown()
    {
        ToggleRadio();
    }

    // 👉 ACTIVACIÓN POR TRIGGER (VR o colisiones)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleRadio();
        }
    }

    // 👉 BOTÓN VR (ENCENDER/APAGAR)
    public void ToggleRadio()
    {
        encendida = !encendida;

        if (encendida)
        {
            Debug.Log("RADIOLA ENCENDIDA");

            if (sfxSource != null && clickSound != null)
                sfxSource.PlayOneShot(clickSound);

            if (musicaSource != null && canciones.Length > 0)
            {
                musicaSource.clip = canciones[indiceActual];
                musicaSource.Play();
            }
        }
        else
        {
            Debug.Log("RADIOLA APAGADA");

            if (musicaSource != null)
                musicaSource.Stop();
        }
    }

    // 👉 BOTÓN VR (CAMBIAR CANCIÓN)
    public void CambiarCancion()
    {
        if (!encendida || canciones.Length == 0) return;

        indiceActual++;

        if (indiceActual >= canciones.Length)
            indiceActual = 0;

        musicaSource.clip = canciones[indiceActual];
        musicaSource.Play();
    }
}