using UnityEngine;

public class Pulsate : MonoBehaviour
{
    [SerializeField] float minScale = 1.9f;
    [SerializeField] float maxScale = 2.1f;
    [SerializeField] float speed = 8f;
    
    void Update()
    {
        float scale = minScale + (Mathf.Sin(Time.time * speed) + 1f) / 2f * (maxScale - minScale);
        transform.localScale = Vector3.one * scale;
    }
}