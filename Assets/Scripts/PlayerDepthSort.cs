using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerDepthSort : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    const int precision = 100;

    private int lastOrder;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponentInParent<Collider2D>();
    }

    void LateUpdate()
    {
        int order =
            -(int)(col.bounds.min.y * precision);

        if (order != lastOrder)
        {
            sr.sortingOrder = order;
            lastOrder = order;
        }
    }
}