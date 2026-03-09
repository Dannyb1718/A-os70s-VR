using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    bool tvEncendida;
    void Start()
    {
        Debug.Log("TVController activo");
    }

    public void ToggleTV()
    {
        tvEncendida = !tvEncendida;

        if (tvEncendida)
        {
            Debug.Log("TV ENCENDIDA");
            videoPlayer.Play();
        }
        else
        {
            Debug.Log("TV APAGADA");
            videoPlayer.Stop();
        }
    }
}
