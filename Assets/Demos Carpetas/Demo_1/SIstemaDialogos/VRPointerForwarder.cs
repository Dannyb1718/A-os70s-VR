using UnityEngine;
using UnityEngine.UI;

#if UNITY_XR_MANAGEMENT || XR_INTERACTION_TOOLKIT
using UnityEngine.XR.Interaction.Toolkit;
#endif

/// <summary>
/// VR integration bridge.
///
/// OPTION A – XR Interaction Toolkit (recommended)
///   - Add an XRRayInteractor to each hand controller.
///   - The World-Space Canvas must use a TrackedDeviceGraphicRaycaster instead
///     of the standard GraphicRaycaster.
///   - With that setup, XRI handles clicks automatically; this script is NOT needed.
///
/// OPTION B – Custom / Legacy VR input (this script)
///   - Attach to the VR controller GameObject.
///   - Assign the CameraPointerInteractor on the Player camera.
///   - The script casts a ray from the controller and forwards select events.
/// </summary>
public class VRPointerForwarder : MonoBehaviour
{
    [Header("References")]
    public CameraPointerInteractor cameraInteractor;

    [Header("Settings")]
    public float rayDistance = 10f;
    public LayerMask uiLayerMask = ~0;

    [Header("VR Input")]
    [Tooltip("XR axis/button name that represents the trigger.")]
    public string vrTriggerButton = "XRI_Right_TriggerButton";

    // Line renderer for visual ray (optional)
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        // Draw ray
        if (_line != null)
        {
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, transform.position + transform.forward * rayDistance);
        }

        // Check for VR trigger input
        bool triggered = false;
        try { triggered = Input.GetButtonDown(vrTriggerButton); }
        catch { /* button not mapped */ }

        if (!triggered) return;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, uiLayerMask))
        {
            Button btn = hit.collider.GetComponentInParent<Button>();
            if (btn != null && cameraInteractor != null)
                cameraInteractor.OnVRSelect(btn);
        }
    }
}
