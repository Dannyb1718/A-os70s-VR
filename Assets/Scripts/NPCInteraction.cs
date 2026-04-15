using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class NPCInteraction : MonoBehaviour
{
    public Animator npcAnimator;
    public float interactionDuration = 3f;

    [Header("Zoom Suave")]
    public float targetFOV = 45f;
    public float zoomSpeed = 5f;

    [Header("VR Ajuste")]
    public float vrLookSpeed = 2f;

    private bool isInteracting = false;
    private bool alreadyInteracted = false; // 🔥 NUEVO

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInteracting && !alreadyInteracted)
        {
            StartCoroutine(InteractionSequence(other.gameObject));
        }
    }

    IEnumerator InteractionSequence(GameObject player)
    {
        isInteracting = true;
        alreadyInteracted = true; // 🔥 SE BLOQUEA PARA SIEMPRE

        var controller = player.GetComponent<FirstPersonController>();
        var rb = player.GetComponent<Rigidbody>();

        bool isVR = XRSettings.enabled;

        // 🔥 STOP TOTAL
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 🔒 BLOQUEO
        if (controller != null)
        {
            controller.playerCanMove = false;
            controller.cameraCanMove = false;
            controller.enableHeadBob = false;
        }

        // 🎭 ANIMACIÓN
        npcAnimator.SetTrigger("Interaccion");

        float timer = 0f;

        while (timer < interactionDuration)
        {
            timer += Time.deltaTime;

            if (controller != null && controller.playerCamera != null)
            {
                if (!isVR)
                {
                    // 🎥 ZOOM SUAVE EN PC
                    controller.playerCamera.fieldOfView = Mathf.Lerp(
                        controller.playerCamera.fieldOfView,
                        targetFOV,
                        Time.deltaTime * zoomSpeed
                    );
                }
                else
                {
                    // 🥽 VR → GIRAR SUAVEMENTE HACIA EL NPC
                    Vector3 direction = transform.position - player.transform.position;
                    direction.y = 0;

                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    player.transform.rotation = Quaternion.Slerp(
                        player.transform.rotation,
                        targetRotation,
                        Time.deltaTime * vrLookSpeed
                    );
                }
            }

            yield return null;
        }

        // 🔄 RESTAURAR

        if (controller != null && controller.playerCamera != null)
        {
            if (!isVR)
            {
                StartCoroutine(RestoreFOV(controller));
            }
        }

        if (controller != null)
        {
            controller.playerCanMove = true;
            controller.cameraCanMove = true;
            controller.enableHeadBob = true;
        }

        isInteracting = false;
    }

    IEnumerator RestoreFOV(FirstPersonController controller)
    {
        while (Mathf.Abs(controller.playerCamera.fieldOfView - controller.fov) > 0.1f)
        {
            controller.playerCamera.fieldOfView = Mathf.Lerp(
                controller.playerCamera.fieldOfView,
                controller.fov,
                Time.deltaTime * zoomSpeed
            );

            yield return null;
        }
    }
}