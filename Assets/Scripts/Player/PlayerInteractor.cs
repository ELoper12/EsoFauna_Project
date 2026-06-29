using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;

    private IInteractable currentTarget;

    private void Update()
    {
        DetectInteractable();
    }

    public void OnInteract()
    {
        currentTarget?.Interact();
    }

    private void DetectInteractable()
    {
        currentTarget = null;

        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = mainCamera.transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, interactDistance, interactLayer))
        {
            Debug.DrawRay(origin, direction*interactDistance, Color.red);
        
        currentTarget = hit.collider.GetComponent<IInteractable>();
        }
    }

    private void OnDrawGizmos()
    {
        if (mainCamera == null)
            return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(
            mainCamera.transform.position,
            mainCamera.transform.position +
            mainCamera.transform.forward * interactDistance);

        Gizmos.DrawWireSphere(
            mainCamera.transform.position +
            mainCamera.transform.forward * interactDistance,
            0.15f);
    }
}