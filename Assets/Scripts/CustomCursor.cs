using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class CustomCursor : MonoBehaviour
{
    [Header("System Cursor")]
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D clickCursorTexture;

    [Header("Animated Cursor")]
    [SerializeField] private RectTransform animatedCursor;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float lowerScale = 0.5f;
    [SerializeField] private float upperScale = 1f;

    public static CustomCursor Instance { get; private set; }

    private Coroutine animationCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        animatedCursor.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetSystemCursor(cursorTexture);
        Cursor.visible = true;
    }

    public static void SetClickCursor()
    {
        if (Instance != null) Instance.StartAnimation(true);
    }

    public static void SetNormalCursor()
    {
        if (Instance != null) Instance.StartAnimation(false);
    }

    private void StartAnimation(bool entering)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimateCursor(entering));
    }
    
    private void MoveToMouse()
    {
        if (Mouse.current == null) return;
        animatedCursor.position = Mouse.current.position.ReadValue();
    }

    private IEnumerator AnimateCursor(bool entering)
    {
        float startScale = animatedCursor.gameObject.activeSelf ? animatedCursor.localScale.x : (entering ? lowerScale : upperScale);
        float targetScale = entering ? upperScale : lowerScale;

        Cursor.visible = false; 
        animatedCursor.gameObject.SetActive(true);
        MoveToMouse();

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / animationDuration));
            animatedCursor.localScale = Vector3.one * Mathf.Lerp(startScale, targetScale, t);
            MoveToMouse();
            yield return null;
        }

        animatedCursor.localScale = Vector3.one * targetScale;
        animatedCursor.gameObject.SetActive(false);

        SetSystemCursor(entering ? clickCursorTexture : cursorTexture);
        Cursor.visible = true;
        animationCoroutine = null;
    }

    private void SetSystemCursor(Texture2D texture)
    {
        Vector2 hotspot = new Vector2(texture.width / 2f, texture.height / 2f);
        Cursor.SetCursor(texture, hotspot, CursorMode.ForceSoftware);
    }

    private void OnDestroy()
    {
        if (Instance != this) return;

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        Cursor.visible = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Instance = null;
    }
}