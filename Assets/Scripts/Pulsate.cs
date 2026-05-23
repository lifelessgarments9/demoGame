using UnityEngine;

public class PulsateUI : MonoBehaviour
{
    [SerializeField] float minScale = 1.9f;
    [SerializeField] float maxScale = 2.1f;
    [SerializeField] float speed = 8f;
    
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float scale = minScale + (Mathf.Sin(Time.time * speed) + 1f) * 0.5f * (maxScale - minScale);
        
        rectTransform.localScale = Vector3.one * scale;
    }
}