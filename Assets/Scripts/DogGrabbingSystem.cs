using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class DogGrabSystem : MonoBehaviour
{
    public Transform mouthTransform; // Reference to the "mouth" transform
    public InfoDisplay infoDisplay; // Reference to the InfoDisplay script for task messages

    private GameObject grabbedObject = null; // Current object in mouth

    public AudioSource audioSource;      // Reference to the AudioSource
    public AudioClip eatSound; 
    public void HandleGrabAction()
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

    public void HandleEatAction()
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
                grabbedObject.transform.rotation *= Quaternion.Euler(0, -62, 0);
                grabbedObject.transform.localPosition = new Vector3(0, 0, 0.5f);


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

            audioSource.PlayOneShot(eatSound);

            Debug.Log("Hot dog eaten!");
        }
    }
}
