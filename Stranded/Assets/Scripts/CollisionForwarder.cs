using UnityEngine;

public class CollisionForwarder : MonoBehaviour
{
    [SerializeField] private CameraController targetController;

    private void Start()
    {
        if (targetController == null)
        {
            Debug.LogError("[CollisionForwarder] Target Controller not assigned!");
        }
        else
        {
            Debug.Log("[CollisionForwarder] Setup complete");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[CollisionForwarder] Trigger Enter detected: {other.gameObject.name}");

        if (targetController != null)
        {
            targetController.SendMessage("HandleExtendedTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[CollisionForwarder] Trigger Exit detected: {other.gameObject.name}");

        if (targetController != null)
        {
            targetController.SendMessage("HandleExtendedTriggerExit", other, SendMessageOptions.DontRequireReceiver);
        }
    }
}
