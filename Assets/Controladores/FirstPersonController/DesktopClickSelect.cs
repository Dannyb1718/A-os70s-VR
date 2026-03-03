using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DesktopClickSelect : MonoBehaviour
{
    private XRRayInteractor rayInteractor;

    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    void Update()
    {
        if (rayInteractor == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            TrySelect();
        }

        if (Input.GetMouseButtonUp(0))
        {
            TryDeselect();
        }
    }

    void TrySelect()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponentInParent<IXRSelectInteractable>();
            var interactor = rayInteractor as IXRSelectInteractor;

            if (interactable != null && interactor != null)
            {
                rayInteractor.interactionManager.SelectEnter(interactor, interactable);
            }
        }
    }

    void TryDeselect()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponentInParent<IXRSelectInteractable>();
            var interactor = rayInteractor as IXRSelectInteractor;

            if (interactable != null && interactor != null)
            {
                rayInteractor.interactionManager.SelectExit(interactor, interactable);
            }
        }
    }
}