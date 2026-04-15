using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class NPCInteraction : MonoBehaviour
{
    public Animator npcAnimator;
    public float interactionDuration = 3f;

    [Header("PC Zoom")]
    public float targetFOV = 45f;
    public float zoomSpeed = 5f;

    [Header("VR Movimiento")]
    public float moveSpeed = 1.5f;
    public float stopDistance = 1.5f;
    public float rotationSpeed = 3f;

    private bool isInteracting = false;
    private bool alreadyInteracted = false;

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
        alreadyInteracted = true;

        var controller = player.GetComponent<FirstPersonController>();
        var rb = player.GetComponent<Rigidbody>();
        var xrOrigin = player.GetComponent<XROrigin>();
        var moveProvider = player.GetComponent<ActionBasedContinuousMoveProvider>();
        var turnProvider = player.GetComponent<ActionBasedContinuousTurnProvider>();

        bool isVR = XRSettings.enabled && xrOrigin != null;

        Camera cam = null;
        Transform originTransform = null;

        if (isVR)
        {
            cam = xrOrigin.Camera;
            originTransform = xrOrigin.Origin.transform;
        }
        else if (controller != null)
        {
            cam = controller.playerCamera;
        }

        // 🔥 BLOQUEO TOTAL REAL
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (controller != null)
        {
            controller.playerCanMove = false;
            controller.cameraCanMove = false;
            controller.enableHeadBob = false;
        }

        if (moveProvider != null) moveProvider.enabled = false;
        if (turnProvider != null) turnProvider.enabled = false;

        // 🎭 ANIMACIÓN
        npcAnimator.SetTrigger("Interaccion");

        float timer = 0f;

        while (timer < interactionDuration)
        {
            timer += Time.deltaTime;

            if (!isVR)
            {
                // 💻 PC → ZOOM
                if (cam != null)
                {
                    cam.fieldOfView = Mathf.Lerp(
                        cam.fieldOfView,
                        targetFOV,
                        Time.deltaTime * zoomSpeed
                    );
                }
            }
            else
            {
                // 🥽 VR → ROTAR + ACERCAR
                Vector3 direction = transform.position - originTransform.position;
                direction.y = 0;

                Quaternion targetRot = Quaternion.LookRotation(direction);

                originTransform.rotation = Quaternion.Slerp(
                    originTransform.rotation,
                    targetRot,
                    Time.deltaTime * rotationSpeed
                );

                float distance = direction.magnitude;

                if (distance > stopDistance)
                {
                    originTransform.position += direction.normalized * moveSpeed * Time.deltaTime;
                }
            }

            yield return null;
        }

        // 🔄 RESTAURAR ZOOM (PC)
        if (!isVR && cam != null && controller != null)
        {
            StartCoroutine(RestoreFOV(cam, controller.fov));
        }

        // 🔓 RESTAURAR TODO
        if (controller != null)
        {
            controller.playerCanMove = true;
            controller.cameraCanMove = true;
            controller.enableHeadBob = true;
        }

        if (moveProvider != null) moveProvider.enabled = true;
        if (turnProvider != null) turnProvider.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        isInteracting = false;
    }

    IEnumerator RestoreFOV(Camera cam, float normalFOV)
    {
        while (Mathf.Abs(cam.fieldOfView - normalFOV) > 0.1f)
        {
            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                normalFOV,
                Time.deltaTime * zoomSpeed
            );

            yield return null;
        }
    }
}