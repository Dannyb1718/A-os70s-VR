using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DesktopXRInteractor : MonoBehaviour
{
    public XRRayInteractor rayInteractor;

    public MonoBehaviour playerController;   // SCRIPT QUE MUEVE LA CÁMARA

    public float scrollSpeed = 2f;
    public float minDistance = 1f;
    public float maxDistance = 10f;

    public float rotationSpeed = 200f;

    private IXRSelectInteractable currentObject;
    private IXRSelectInteractor interactor;

    private float currentDistance;
    private bool rotating = false;

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
        HandleRotation();
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
        if (currentObject == null || rotating)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            currentDistance += scroll * scrollSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            rayInteractor.attachTransform.localPosition =
                new Vector3(0, 0, currentDistance);
        }
    }

    void HandleRotation()
    {
        if (currentObject == null)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            rotating = true;

            if (playerController != null)
                playerController.enabled = false;
        }

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            rayInteractor.attachTransform.Rotate(
                Vector3.up,
                mouseX * rotationSpeed * Time.deltaTime,
                Space.World
            );

            rayInteractor.attachTransform.Rotate(
                Vector3.right,
                -mouseY * rotationSpeed * Time.deltaTime,
                Space.Self
            );
        }

        if (Input.GetMouseButtonUp(1))
        {
            rotating = false;

            if (playerController != null)
                playerController.enabled = true;
        }
    }
}