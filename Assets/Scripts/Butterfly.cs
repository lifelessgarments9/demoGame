using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName = "butterfly";
    private AudioSource audioSource;

    void Awake()
    {
        animator.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        animator.enabled = true;
        animator.Play(animationName, 0, 0);
        audioSource.Play();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        animator.enabled = false;
        audioSource.Stop();
    }
}