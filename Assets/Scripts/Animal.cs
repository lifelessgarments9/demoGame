using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private string animationName = "Hide";
    private AudioSource audioSource;
    [SerializeField] private AudioClip sound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play(animationName);
            audioSource.PlayOneShot(sound);
        }
    }

}
