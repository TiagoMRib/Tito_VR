using UnityEngine;
using UnityEngine.InputSystem;

public class DogGrabSystem : MonoBehaviour
{
    public Transform mouthTransform; // Reference to the "mouth" transform
    public InputActionReference grabAction; // Reference to the input action
    private GameObject grabbedObject = null; // Current object in mouth

    void OnEnable()
    {
        // Enable the input action
        grabAction.action.Enable();
        grabAction.action.performed += HandleGrabAction;
    }

    void OnDisable()
    {
        // Disable the input action
        grabAction.action.Disable();
        grabAction.action.performed -= HandleGrabAction;
    }

    private void HandleGrabAction(InputAction.CallbackContext context)
    {
        Debug.Log("Grab action performed!");
        if (grabbedObject == null)
        {
            TryGrabObject();
        }
        else
        {
            DropObject();
        }
    }

    void TryGrabObject()
    {
        // Detect nearby objects with the "Grabbable" tag
        Collider[] nearbyObjects = Physics.OverlapSphere(mouthTransform.position, 0.5f); // Adjust radius~
        foreach (Collider collider in nearbyObjects)
        {
            Debug.Log("Detected object: " + collider.gameObject.name);
            if (collider.CompareTag("Grabbable"))
            {
                Debug.Log("Grabbed object: " + collider.gameObject.name);
                // Grab the object
                grabbedObject = collider.gameObject;
                grabbedObject.transform.SetParent(mouthTransform);
                grabbedObject.transform.localPosition = Vector3.zero;
                grabbedObject.transform.localRotation = Quaternion.identity;

                // Disable physics while in mouth
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                break;
            }
        }
    }

    void DropObject()
    {
        if (grabbedObject != null)
        {
            // Re-enable physics
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Detach the object
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }
}
