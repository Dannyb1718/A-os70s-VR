using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // El VideoPlayer ya existente
    public AudioSource audioSource;  // El AudioSource que reproducirá el sonido
    public AudioClip clickSound;     // El sonido que se reproducirá al hacer clic
    bool tvEncendida;

    void Start()
    {
        Debug.Log("TVController activo");

        // Asegurarse de que el VideoPlayer y el AudioSource no estén reproduciendo al inicio
        videoPlayer.Stop();
        audioSource.Stop();
    }

    public void ToggleTV()
    {
        tvEncendida = !tvEncendida;

        if (tvEncendida)
        {
            Debug.Log("TV ENCENDIDA");

            // Reproducir el sonido del clic **solo cuando se enciende la TV**
            audioSource.PlayOneShot(clickSound); // Reproducir solo el clic

            // Vincular el AudioSource al VideoPlayer para que reproduzca el audio
            videoPlayer.Play();
            audioSource.Play();  // Reproducir el sonido del video
        }
        else
        {
            Debug.Log("TV APAGADA");

            // Detener el video y el sonido
            videoPlayer.Stop();
            audioSource.Stop();  // Detener el sonido del video
        }
    }
}