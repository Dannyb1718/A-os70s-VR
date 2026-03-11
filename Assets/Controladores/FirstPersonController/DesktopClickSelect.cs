using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DesktopXRInteractor : MonoBehaviour
{
    public XRRayInteractor rayInteractor;

    public float scrollSpeed = 2f;
    public float minDistance = 1f;
    public float maxDistance = 10f;

    private IXRSelectInteractable currentObject;
    private IXRSelectInteractor interactor;

    private float currentDistance;

    void Start()
    {
        interactor = rayInteractor as IXRSelectInteractor;
    }

    void Update()
    {
        if (rayInteractor == null)
            return;

        HandleGrab();
        HandleScroll();
    }

    void HandleGrab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                var interactable = hit.collider.GetComponentInParent<IXRSelectInteractable>();

                if (interactable != null)
                {
                    currentObject = interactable;

                    rayInteractor.interactionManager.SelectEnter(interactor, interactable);

                    currentDistance = hit.distance;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentObject != null)
            {
                rayInteractor.interactionManager.SelectExit(interactor, currentObject);
                currentObject = null;
            }
        }
    }

    void HandleScroll()
    {
        if (currentObject == null)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            currentDistance += scroll * scrollSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            rayInteractor.attachTransform.localPosition = new Vector3(0, 0, currentDistance);
        }
    }
}