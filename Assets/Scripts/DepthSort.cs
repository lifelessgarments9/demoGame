using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSort : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    const int precision = 100;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponentInParent<Collider2D>();

        float referenceY = col.bounds.min.y;
        sr.sortingOrder = -(int)(referenceY * precision);
    }
}