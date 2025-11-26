using UnityEngine;

public class OutlineRaycast : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 100f;
    private Outline lastOutlinedObject;
    public PlayerInteract playerInteract;
    public MyEnemyAI myEnemyAI;

    void Update()
    {
        CheckOutline();
    }

    void CheckOutline()
    {
        // Perform the raycast
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Debug: Print the name of the object hit
            //Debug.Log("Hit object: " + hit.collider.gameObject.name);

            // Check if the hit object has the tag "OutlineTrigger"
            if (hit.collider.CompareTag("OutlineTrigger"))
            {
                //Debug.Log("Object has OutlineTrigger tag");

                // Get the parent or the target object with the Outline component
                Outline outline = hit.collider.GetComponentInParent<Outline>();
                Gramophone gramophone = hit.collider.GetComponentInParent<Gramophone>();
                Record record = hit.collider.GetComponentInParent<Record>();
                Acorn acorn = hit.collider.GetComponentInParent<Acorn>();

                if (outline != null)
                {
                    //Debug.Log("Outline component found on: " + outline.gameObject.name);

                    // Check if it's a Gramophone and if the record is placed
                    if (gramophone != null && gramophone.isRecordPlaced)
                    {
                        Debug.Log("Gramophone record is already placed, no outline.");
                        if (lastOutlinedObject != null)
                        {
                            lastOutlinedObject.enabled = false;
                            lastOutlinedObject = null;
                        }
                        return;
                    }

                    // If it's not a gramophone with a record placed, or it's a record or acorn
                    if (record != null || acorn != null)
                    {
                        // Ensure outline for records and acorns is always enabled
                        if (lastOutlinedObject != null && lastOutlinedObject != outline)
                        {
                            //Debug.Log("Disabling last outlined object: " + lastOutlinedObject.gameObject.name);
                            lastOutlinedObject.enabled = false;
                        }

                        //Debug.Log("Enabling outline on: " + outline.gameObject.name);
                        outline.enabled = true;
                        lastOutlinedObject = outline;
                        return;
                    }

                    // If music is playing, disable any currently outlined object and return
                    if (myEnemyAI.musicPlaying)
                    {
                        if (lastOutlinedObject != null)
                        {
                            //Debug.Log("Music is playing, disabling outline on: " + lastOutlinedObject.gameObject.name);
                            lastOutlinedObject.enabled = false;
                            lastOutlinedObject = null;
                        }
                        return;
                    }

                    // Enable outline for other objects if Outline component exists
                    if (lastOutlinedObject != null && lastOutlinedObject != outline)
                    {
                        //Debug.Log("Disabling last outlined object: " + lastOutlinedObject.gameObject.name);
                        lastOutlinedObject.enabled = false;
                    }

                    //Debug.Log("Enabling outline on: " + outline.gameObject.name);
                    outline.enabled = true;
                    lastOutlinedObject = outline;
                }
                else
                {
                    Debug.Log("No Outline component found on parent objects.");
                }
            }
            else if (lastOutlinedObject != null)
            {
                Debug.Log("No OutlineTrigger tag hit, disabling last outlined object: " + lastOutlinedObject.gameObject.name);
                lastOutlinedObject.enabled = false;
                lastOutlinedObject = null;
            }
        }
        else if (lastOutlinedObject != null)
        {
            Debug.Log("No object hit, disabling last outlined object: " + lastOutlinedObject.gameObject.name);
            lastOutlinedObject.enabled = false;
            lastOutlinedObject = null;
        }
    }
}
