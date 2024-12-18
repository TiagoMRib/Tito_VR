using UnityEngine;
//using UnityEngine.InputSystem;

public class DogGrabSystem : MonoBehaviour
{
    /*
    public Transform mouthTransform; // Reference to the "mouth" transform
    public InputActionReference grabAction; // Reference to the grab input action
    public InputActionReference eatAction; // Reference to the eat input action
    public InfoDisplay infoDisplay; // Reference to the InfoDisplay script for task messages

    private GameObject grabbedObject = null; // Current object in mouth

    void OnEnable()
    {
        // Enable input actions
        grabAction.action.Enable();
        grabAction.action.performed += HandleGrabAction;

        eatAction.action.Enable();
        eatAction.action.performed += HandleEatAction;
    }

    void OnDisable()
    {
        // Disable input actions
        grabAction.action.Disable();
        grabAction.action.performed -= HandleGrabAction;

        eatAction.action.Disable();
        eatAction.action.performed -= HandleEatAction;
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

    private void HandleEatAction(InputAction.CallbackContext context)
    {
        if (grabbedObject != null)
        {
            Debug.Log("Eat action performed!");
            EatObject();
        }
    }

    void TryGrabObject()
    {
        // Detect nearby objects with the "Grabbable" tag
        Collider[] nearbyObjects = Physics.OverlapSphere(mouthTransform.position, 0.5f); // Adjust radius
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

                // Disable physics while in mouth
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                // Update the task message
                infoDisplay.ShowMessage("Find a safe place to eat the hot dog!");

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

            // Clear the message when dropping the object
            infoDisplay.ShowMessage("You dropped the hot dog!");
        }
    }

    void EatObject()
    {
        if (grabbedObject != null)
        {
            // Destroy the object
            Destroy(grabbedObject);

            // Clear the task message
            infoDisplay.ShowMessage("");

            grabbedObject = null;

            Debug.Log("Hot dog eaten!");
        }
    }
    */
}
