using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class ImagePanController : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private RectTransform content;

    private RectTransform viewport;
    private Vector2 startPosition;

    private void Awake()
    {
        viewport = GetComponent<RectTransform>();
        startPosition = content.anchoredPosition;
        enabled = false;
    }

    public void SetViewMode(bool active, bool resetPosition = true)
    {
        enabled = active;
        if (resetPosition) content.anchoredPosition = startPosition;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, mousePos, null, out Vector2 mouse))
            return;

        Rect viewRect = viewport.rect;

        float x = Mathf.Clamp(mouse.x / (viewRect.width * 0.5f), -1f, 1f);
        float y = Mathf.Clamp(mouse.y / (viewRect.height * 0.5f), -1f, 1f);

        float maxX = Mathf.Max(0f, (content.rect.width - viewRect.width) * 0.5f);
        float maxY = Mathf.Max(0f, (content.rect.height - viewRect.height) * 0.5f);

        content.anchoredPosition = startPosition + new Vector2(-x * maxX * sensitivity, -y * maxY * sensitivity);
    }
}