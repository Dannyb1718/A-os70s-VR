using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject menuPrincipal;
    public GameObject configuraciones;
    public GameObject tutorial;

    public FadeManager fadeManager;

    void Update()
    {
        if (startScreen.activeSelf && 
           (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            StartCoroutine(CambiarPantalla(ShowMenu));
        }
    }

    public void ShowComienzo()
    {
        startScreen.SetActive(true);
        menuPrincipal.SetActive(false);
        configuraciones.SetActive(false);
        tutorial.SetActive(false);
    }

    public void ShowMenu()
    {
        startScreen.SetActive(false);
        menuPrincipal.SetActive(true);
        configuraciones.SetActive(false);
        tutorial.SetActive(false);
    }

    public void ShowConfiguraciones()
    {
        startScreen.SetActive(false);
        menuPrincipal.SetActive(false);
        configuraciones.SetActive(true);
        tutorial.SetActive(false);
    }

    public void ShowTutorial()
    {
        startScreen.SetActive(false);
        menuPrincipal.SetActive(false);
        configuraciones.SetActive(false);
        tutorial.SetActive(true);
    }

    IEnumerator CambiarPantalla(System.Action accion)
    {
        yield return StartCoroutine(fadeManager.FadeOut());

        accion.Invoke();

        yield return StartCoroutine(fadeManager.FadeIn());
    }

    // Métodos para botones
    public void IrMenu() => StartCoroutine(CambiarPantalla(ShowMenu));
    public void IrConfiguraciones() => StartCoroutine(CambiarPantalla(ShowConfiguraciones));
    public void IrTutorial() => StartCoroutine(CambiarPantalla(ShowTutorial));
    public void IrComienzo() => StartCoroutine(CambiarPantalla(ShowComienzo));
}