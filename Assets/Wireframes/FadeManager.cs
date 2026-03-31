using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut(Sprite nuevaImagen = null)
    {
        // 🔥 Cambiar imagen antes del fade
        if (nuevaImagen != null)
        {
            fadeImage.sprite = nuevaImagen;
            fadeImage.color = new Color(1, 1, 1, 0); // invisible pero lista
        }

        float t = 0;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float t = 0;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
}