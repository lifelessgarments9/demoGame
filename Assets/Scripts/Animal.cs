using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private string animationName = "Hide";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play(animationName);
        }
    }

    public void DisableSelf()
    {
        Destroy(gameObject);
    }
}
