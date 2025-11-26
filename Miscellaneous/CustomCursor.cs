using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;         // Your default cursor texture
    public Texture2D cursorPressedTexture;  // Cursor texture when pressed
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        // Set the initial cursor
        ChangeCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button is held down
        {
            // Change to the pressed cursor
            ChangeCursor(cursorPressedTexture, hotSpot, cursorMode);
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button is released
        {
            // Reset to the default cursor
            ChangeCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    public void ChangeCursor(Texture2D texture, Vector2 hotSpot, CursorMode mode)
    {
        Cursor.SetCursor(texture, hotSpot, mode);
    }

    public void ResetCursor()
    {
        ChangeCursor(cursorTexture, hotSpot, cursorMode);
    }
}
