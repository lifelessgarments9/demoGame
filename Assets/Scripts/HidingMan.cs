using UnityEngine;

public class HidingMan : MonoBehaviour
{
    Animator animator;
    [SerializeField] private string animationName = "Hide";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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