using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

[System.Serializable]
public class AnimationStep
{
    public string stateName;
    public float duration;
}

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC")]
    public Animator npcAnimator;

    [Header("Secuencia de Animaciones (CONTROL REAL)")]
    public List<AnimationStep> animationSequence = new List<AnimationStep>();

    [Header("HUD")]
    public TextMeshProUGUI itemMessageText;
    public float messageDuration = 2.5f;

    private bool isInteracting = false;
    private bool yaInteractuo = false;
    private bool tieneItem = false;

    void Start()
    {
        if (itemMessageText != null)
            itemMessageText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isInteracting || yaInteractuo) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(InteractionSequence(other.gameObject));
        }
    }

    IEnumerator InteractionSequence(GameObject player)
    {
        isInteracting = true;

        var controller = player.GetComponent<FirstPersonController>();
        var rb = player.GetComponent<Rigidbody>();

        // 🔥 CONGELAR TODO (COMO TU CÓDIGO ORIGINAL)
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (controller != null)
        {
            controller.playerCanMove = false;
            controller.cameraCanMove = false;
            controller.enableHeadBob = false;
        }

        yield return null;

        // =========================
        // 🎭 SECUENCIA REAL (LA BUENA)
        // =========================
        foreach (AnimationStep step in animationSequence)
        {
            if (npcAnimator != null && !string.IsNullOrEmpty(step.stateName))
            {
                Debug.Log("Reproduciendo: " + step.stateName);

                // 🔥 ESTO ES LO QUE HACE QUE FUNCIONE
                npcAnimator.Play(step.stateName, 0, 0f);
            }

            yield return new WaitForSeconds(step.duration);
        }

        // 🎁 ITEM
        if (!tieneItem)
        {
            tieneItem = true;
            Debug.Log("ITEM OBTENIDO ✅");

            if (itemMessageText != null)
                StartCoroutine(MostrarMensajeHUD("🎁 Nuevo ítem obtenido"));
        }

        // 🔥 RESTAURAR SIN DESLIZAMIENTO
        if (controller != null)
        {
            controller.playerCanMove = true;
            controller.enableHeadBob = true;
        }

        yield return null; // 🔥 limpia input (CLAVE)

        if (controller != null)
        {
            controller.cameraCanMove = true;
        }

        if (rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        yaInteractuo = true;

        // 🔒 opcional: desactivar trigger
        GetComponent<Collider>().enabled = false;

        isInteracting = false;
    }

    IEnumerator MostrarMensajeHUD(string mensaje)
    {
        itemMessageText.text = mensaje;
        itemMessageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        itemMessageText.gameObject.SetActive(false);
    }
}