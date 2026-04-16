using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[System.Serializable]
public class AnimationStep
{
    public string stateName;   // nombre del estado en el Animator
    public float duration;
}

public class NPCInteraction : MonoBehaviour
{
    public Animator npcAnimator;
    public List<AnimationStep> animationSequence = new List<AnimationStep>();

    private bool isInteracting = false;
    private Vector3 originalPosition;

    void Start()
    {
        if (npcAnimator != null)
        {
            npcAnimator.applyRootMotion = false; // 🔥 evita movimiento raro del NPC
        }

        originalPosition = transform.position;
    }

    void LateUpdate()
    {
        // BLOQUEA NPC EN Y (no se hunda ni flote)
        transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInteracting)
        {
            StartCoroutine(InteractionSequence(other.gameObject));
        }
    }

    IEnumerator InteractionSequence(GameObject player)
    {
        isInteracting = true;

        var controller = player.GetComponent<FirstPersonController>();
        var rb = player.GetComponent<Rigidbody>();
        var moveProvider = player.GetComponent<DynamicMoveProvider>();

        // =========================
        // BLOQUEO TOTAL PLAYER
        // =========================
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;

            // 💥 congela TODO (XYZ + rotaciones)
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (controller != null)
        {
            controller.playerCanMove = false;
            controller.cameraCanMove = false;
            controller.enableHeadBob = false;

            // 💥 desactiva completamente el controlador
            controller.enabled = false;
        }

        if (moveProvider != null)
        {
            moveProvider.enabled = false;
        }

        yield return null;

        // =========================
        //  SECUENCIA DE ANIMACIONES
        // =========================
        foreach (AnimationStep step in animationSequence)
        {
            if (npcAnimator != null && !string.IsNullOrEmpty(step.stateName))
            {
                Debug.Log("Reproduciendo: " + step.stateName);

                npcAnimator.Play(step.stateName, 0, 0f);
            }

            yield return new WaitForSeconds(step.duration);
        }

        // =========================
        //  RESTAURAR PLAYER
        // =========================
        if (rb != null)
        {
            rb.useGravity = true;

            //  quitar congelamiento
            rb.constraints = RigidbodyConstraints.None;
        }

        if (controller != null)
        {
            controller.enabled = true;

            controller.playerCanMove = true;
            controller.cameraCanMove = true;
            controller.enableHeadBob = true;
        }

        if (moveProvider != null)
        {
            moveProvider.enabled = true;
        }

        isInteracting = false;
    }
}