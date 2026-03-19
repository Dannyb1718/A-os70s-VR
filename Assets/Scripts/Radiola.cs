using UnityEngine;

public class RadiolaController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource musicaSource;   // Música principal
    public AudioSource sfxSource;      // Sonido de clic
    public AudioClip clickSound;       // Sonido al encender

    private bool encendida = false;

    void Start()
    {
        Debug.Log("Radiola lista");

        if (musicaSource != null)
            musicaSource.Stop();

        if (sfxSource != null)
            sfxSource.Stop();
    }

    // 👉 CLICK CON MOUSE (para pruebas en PC)
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

    // 👉 FUNCIÓN PRINCIPAL
    public void ToggleRadio()
    {
        encendida = !encendida;

        if (encendida)
        {
            Debug.Log("RADIOLA ENCENDIDA");

            if (sfxSource != null && clickSound != null)
                sfxSource.PlayOneShot(clickSound);

            if (musicaSource != null)
                musicaSource.Play();
        }
        else
        {
            Debug.Log("RADIOLA APAGADA");

            if (musicaSource != null)
                musicaSource.Stop();
        }
    }
}