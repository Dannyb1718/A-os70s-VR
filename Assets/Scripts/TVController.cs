using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    bool tvEncendida = false;

    public void ToggleTV()
    {
        Debug.Log("Se presionó la TV");

        if (tvEncendida == false)
        {
            Debug.Log("TV ENCENDIDA");
            videoPlayer.Play();
            tvEncendida = true;
        }
        else
        {
            Debug.Log("TV APAGADA");
            videoPlayer.Stop();
            tvEncendida = false;
        }
    }
}