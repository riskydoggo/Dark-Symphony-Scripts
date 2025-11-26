using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    public Transform holdPoint;
    public LayerMask interactableLayer; // Layer for all interactable items (e.g., records and acorns)
    public LayerMask gramophoneLayer;   // Layer for gramophones
    public GameObject heldItem = null, recordText, gramText;
    public GameObject numRecordsTextObj, pickupText0, pickupText1, pickupText2, pickupText3, pickupText4, pickupText5;
    private Vector3 originalScale;
    public AudioSource[] recordClips;
    public AudioSource vinylCollectionSound;
    public bool firstRecord = true;
    public bool found0 = false, found1=false, found2=false, found3=false, found4=false, found5 = false;
    public bool isRecordsInCorrectOrder = false;
    public int currentRecordIndexPlaying = -1;
    public int currentGramophoneIndexPlaying = -1;
    public int numRecordsFound;
    public TextMeshProUGUI numRecordsText;

    public TimerManager timerManager;
    public MyEnemyAI myEnemyAI;

    void Start()
    {
        // Ensure timerManager is assigned
        timerManager = FindObjectOfType<TimerManager>();

        // Check if recordClips are assigned
        for (int i = 0; i < recordClips.Length; i++)
        {
            if (recordClips[i] == null)
            {
                Debug.LogError("AudioSource at index " + i + " is not assigned.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");

            if (heldItem == null)
            {
                TryPickUpItem();
            }
            else
            {
                if (IsLookingAtGramophone(out Gramophone gramophone))
                {
                    PlaceRecordOnGramophone(gramophone);
                }
                else
                {
                    DropItem();
                }
            }
        }

        if (heldItem != null && Input.GetMouseButtonDown(0))
        {
            ThrowItem();
        }

        if (IsLookingAtGramophone(out _) && firstRecord)
        {
            if (heldItem != null && heldItem.name.Contains("Record"))
            {
                recordText.SetActive(true);
            }
            else
            {
                gramText.SetActive(true);
            }
        }
        else
        {
            recordText.SetActive(false);
            gramText.SetActive(false);
        }
    }

    bool IsLookingAtGramophone(out Gramophone gramophone)
    {
        gramophone = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, gramophoneLayer, QueryTriggerInteraction.Ignore))
        {
            Gramophone g = hit.collider.GetComponent<Gramophone>();
            if (g != null)
            {
                gramophone = g;
                return true;
            }
        }

        return false;
    }

    void TryPickUpItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.SphereCast(ray, 0.5f, out RaycastHit hit, interactRange, interactableLayer, QueryTriggerInteraction.Ignore))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                HoldItem(hit.collider.gameObject);
            }
        }
    }

    public void HoldItem(GameObject item)
    {
        heldItem = item;
        originalScale = heldItem.transform.localScale;
        heldItem.transform.SetParent(holdPoint);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;
        heldItem.transform.localScale = originalScale;

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false; // Prevents collisions while being held
        }

        // Increment records and set the UI text to display the number of records found
        if (heldItem.name == "Record 0" && found0 == false)
        {
            found0 = true;
            numRecordsFound += 1;
            numRecordsText.text = numRecordsFound.ToString() + "/6";
            numRecordsText.color = new Color(numRecordsText.color.r, numRecordsText.color.g, numRecordsText.color.b, 1);
            vinylCollectionSound.Play();

            numRecordsTextObj.SetActive(true);
            pickupText0.SetActive(true); // Turn Canvas On

            // Reset alpha and start fading out again
            numRecordsText.GetComponent<FadeAway>().ResetAlphaAndFade();
        }
        else if (heldItem.name == "Record 1" && found1 == false)
        {
            found1 = true;
            numRecordsFound += 1;
            numRecordsText.text = numRecordsFound.ToString() + "/6";
            numRecordsText.color = new Color(numRecordsText.color.r, numRecordsText.color.g, numRecordsText.color.b, 1);
            vinylCollectionSound.Play();

            numRecordsTextObj.SetActive(true);
            pickupText1.SetActive(true); // Turn Canvas On

            // Reset alpha and start fading out again
            numRecordsText.GetComponent<FadeAway>().ResetAlphaAndFade();
        }
        // Repeat similar steps for other records...
        else if (heldItem.name == "Record 2" && found2 == false)
        {
            found2 = true;
            numRecordsFound += 1;
            numRecordsText.text = numRecordsFound.ToString() + "/6";
            numRecordsText.color = new Color(numRecordsText.color.r, numRecordsText.color.g, numRecordsText.color.b, 1);
            vinylCollectionSound.Play();

            numRecordsTextObj.SetActive(true);
            pickupText2.SetActive(true); // Turn Canvas On

            // Reset alpha and start fading out again
            numRecordsText.GetComponent<FadeAway>().ResetAlphaAndFade();
        }
        // Continue for other records...
        else if (heldItem.name == "Record 3" && found3 == false)
        {
            found3 = true;
            numRecordsFound += 1;

            numRecordsText.text = numRecordsFound.ToString() + "/6";
            numRecordsText.color = new Color(numRecordsText.color.r, numRecordsText.color.g, numRecordsText.color.b, 1);
            vinylCollectionSound.Play();

            numRecordsTextObj.SetActive(true);
            pickupText3.SetActive(true);//Turn Canvas On

            // Reset alpha and start fading out again
            numRecordsText.GetComponent<FadeAway>().ResetAlphaAndFade();
        }
        else if (heldItem.name == "Record 4" && found4 == false)
        {
            found4 = true;
            numRecordsFound += 1;

            numRecordsText.text = numRecordsFound.ToString() + "/6";
            numRecordsText.color = new Color(numRecordsText.color.r, numRecordsText.color.g, numRecordsText.color.b, 1);
            vinylCollectionSound.Play();

            numRecordsTextObj.SetActive(true);
            pickupText4.SetActive(true);//Turn Canvas On

            // Reset alpha and start fading out again
            numRecordsText.GetComponent<FadeAway>().ResetAlphaAndFade();
        }
        else if (heldItem.name == "Record 5" && found5 == false)
        {
            found5 = true;
            numRecordsFound += 1;

            numRecordsText.text = numRecordsFound.ToString() + "/6";
            numRecordsText.color = new Color(numRecordsText.color.r, numRecordsText.color.g, numRecordsText.color.b, 1);
            vinylCollectionSound.Play();

            numRecordsTextObj.SetActive(true);
            pickupText5.SetActive(true);//Turn Canvas On

            // Reset alpha and start fading out again
            numRecordsText.GetComponent<FadeAway>().ResetAlphaAndFade();
        }
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            IInteractable interactable = heldItem.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Record record = heldItem.GetComponent<Record>();
                if (record != null)
                {
                    record.isHeld = false;
                }
                Acorn acorn = heldItem.GetComponent<Acorn>();
                if (acorn != null)
                {
                    acorn.isHeld = false;
                }
            }

            heldItem.transform.SetParent(null);
            heldItem.transform.localScale = originalScale;

            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true; // Re-enable collisions when dropped
            }

            heldItem = null;
        }
    }

    void ThrowItem()
    {
        if (heldItem != null)
        {
            IInteractable interactable = heldItem.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Record record = heldItem.GetComponent<Record>();
                if (record != null)
                {
                    record.isHeld = false;
                }
                Acorn acorn = heldItem.GetComponent<Acorn>();
                if (acorn != null)
                {
                    acorn.isHeld = false;
                }
            }

            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
                heldItem.transform.SetParent(null);
                heldItem.transform.localScale = originalScale;
                rb.AddForce(transform.forward * 15f, ForceMode.Impulse); // Adjust the force as needed
            }

            heldItem = null;
        }
    }

    void PlaceRecordOnGramophone(Gramophone gramophone)
    {
        Animation animation = gramophone.GetComponent<Animation>();

        if (heldItem != null && !myEnemyAI.musicPlaying)
        {
            Record record = heldItem.GetComponent<Record>();
            if (record != null)
            {
                if (heldItem.name == "Record 0")
                {
                    currentRecordIndexPlaying = 0;
                }
                else if (heldItem.name == "Record 1")
                {
                    currentRecordIndexPlaying = 1;
                }
                else if (heldItem.name == "Record 2")
                {
                    currentRecordIndexPlaying = 2;
                }
                else if (heldItem.name == "Record 3")
                {
                    currentRecordIndexPlaying = 3;
                }
                else if (heldItem.name == "Record 4")
                {
                    currentRecordIndexPlaying = 4;
                }
                else if (heldItem.name == "Record 5")
                {
                    currentRecordIndexPlaying = 5;
                    Debug.LogWarning("Record 5 placed");
                }

                heldItem.transform.SetParent(gramophone.transform);
                heldItem.transform.localPosition = new Vector3(0, 0.268f, 0);
                heldItem.transform.localRotation = Quaternion.identity;

                animation.Play(); //Gramophone handle begins to turn
                startMusicManager();

                //myEnemyAI.AIStateController(2);
                

                currentGramophoneIndexPlaying = gramophone.gramophoneID;
                firstRecord = false;

                if (currentRecordIndexPlaying == currentGramophoneIndexPlaying)
                {
                    isRecordsInCorrectOrder = true;
                }
                else
                {
                    isRecordsInCorrectOrder = false;
                }

                heldItem = null;
            }
        }
    }

    // Called when the player places a record onto a gramophone
    void startMusicManager()
    {
        // Tell the enemy AI that music is currently playing
        myEnemyAI.musicPlaying = true;

        // Debug log to confirm record placement and current AI state
        Debug.Log($"[Player] Placed record ? musicPlaying = {myEnemyAI.musicPlaying}; AI state was {myEnemyAI.currentAction}");

        // Get the length of the current record's audio clip
        float recordClipLength = recordClips[currentRecordIndexPlaying].clip.length;

        // Special case: extend the first record’s playback length by 3 seconds
        if (currentRecordIndexPlaying == 0)
        {
            recordClipLength += 3f;
        }

        // Start playing the audio for the current record
        recordClips[currentRecordIndexPlaying].Play();

        // Debug log to show which record started playing
        Debug.LogWarning("Record " + currentRecordIndexPlaying + " Playing NOW!");

        // Start a countdown timer for the length of the record
        // When the timer completes, OnTimerComplete() will be called
        timerManager.StartTimer(recordClipLength, OnTimerComplete);
    }

    // Called automatically when the record's timer finishes
    void OnTimerComplete()
    {
        // Tell the enemy AI that the music has stopped
        myEnemyAI.musicPlaying = false;
    }

}