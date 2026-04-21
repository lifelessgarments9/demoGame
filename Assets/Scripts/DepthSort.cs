using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSort : MonoBehaviour
{
    SpriteRenderer sr;
    Collider2D col;
    const int precision = 100;
    private const float delay = 1f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponentInParent<Collider2D>();
    }

    void LateUpdate()
    {
        float referenceY = col.bounds.min.y;

        sr.sortingOrder = -(int)(referenceY * precision);
    }
}