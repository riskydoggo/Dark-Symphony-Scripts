using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    private Record pickedUpRecord;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickupRecord(Record record)
    {
        if (pickedUpRecord == null)
        {
            pickedUpRecord = record;
            record.gameObject.SetActive(false);
        }
    }

    public bool HasRecord()
    {
        return pickedUpRecord != null;
    }

    public Record GetPickedUpRecord()
    {
        return pickedUpRecord;
    }

    public void ClearPickedUpRecord()
    {
        pickedUpRecord = null;
    }
}
