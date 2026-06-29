using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hill : MonoBehaviour
{
    [SerializeField] private float maxSlope = 0.8f;

    private Collider2D trigger;

    void Awake()
    {
        trigger = GetComponent<Collider2D>();
    }

    public Vector2 ModifyDirection(
        Vector2 playerPos,
        Vector2 direction)
    {
        if (direction.x == 0f)
            return direction;
//доля пути!
        float t = Mathf.InverseLerp(
            trigger.bounds.min.x,
            trigger.bounds.max.x,
            playerPos.x);

        // Производная синуса — наклон дуги
        float slope = Mathf.Cos(t * Mathf.PI) * maxSlope;

        direction.y += slope * Mathf.Sign(direction.x);

        return direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Player.Instance.SetHill(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Player.Instance.ClearHill(this);
    }
}