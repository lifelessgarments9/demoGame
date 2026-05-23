using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StaticDepthSortComposite : MonoBehaviour
{
    private const int precision = 100;

    private Collider2D col;
    private SpriteRenderer[] spriteRenderers;

    void Awake()
    {
        col = GetComponent<Collider2D>();

        spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>(true);

        if (spriteRenderers.Length == 0)
        {
            Debug.LogWarning(
                $"StaticDepthSortComposite: Не найдено SpriteRenderer на объекте {gameObject.name}",
                this
            );

            return;
        }

        float referenceY = col.bounds.min.y;

        int sortingOrder =
            -(int)(referenceY * precision);

        foreach (var sr in spriteRenderers)
        {
            sr.sortingOrder = sortingOrder;
        }
    }
}