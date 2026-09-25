using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    private void Start()
    {
        Vector2 hotspot = new Vector2(
            cursorTexture.width / 2f,
            cursorTexture.height / 2f
        );

        UnityEngine.Cursor.SetCursor(
            cursorTexture,
            hotspot,
            CursorMode.Auto
        );
    }
}