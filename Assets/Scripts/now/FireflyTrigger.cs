using UnityEngine;

public class FireflyTrigger : MonoBehaviour
{
    [SerializeField] private Firefly[] fireflies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        foreach (var f in fireflies)
        {
            f.gameObject.SetActive(true);
            f.Activate(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        foreach (var f in fireflies)
        {
            f.Deactivate();
        }
    }
}