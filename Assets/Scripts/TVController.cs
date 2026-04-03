using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // El VideoPlayer ya existente
    public AudioClip clickSound;     // Sonido del clic

    public VideoClip[] videos;       // Lista de videos
    private int indiceActual = 0;

    public AudioSource audio3D;      // 🔥 AudioSource 3D

    bool tvEncendida;

    void Start()
    {
        Debug.Log("TVController activo");

        videoPlayer.Stop();

        // 🔥 Vincular audio del video al AudioSource 3D
        if (audio3D != null)
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.SetTargetAudioSource(0, audio3D);
        }
    }

    void Update()
    {
        // Clic derecho → cambiar video SOLO si está encendida
        if (tvEncendida && Input.GetMouseButtonDown(1))
        {
            CambiarVideo();
        }
    }

    public void ToggleTV()
    {
        tvEncendida = !tvEncendida;

        if (tvEncendida)
        {
            Debug.Log("TV ENCENDIDA");

            // Sonido de clic
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);

            videoPlayer.clip = videos[indiceActual];
            videoPlayer.Play();
        }
        else
        {
            Debug.Log("TV APAGADA");

            videoPlayer.Stop();
        }
    }

    void CambiarVideo()
    {
        indiceActual++;

        if (indiceActual >= videos.Length)
            indiceActual = 0;

        videoPlayer.clip = videos[indiceActual];
        videoPlayer.Play();
    }
}