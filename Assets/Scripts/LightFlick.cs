using UnityEngine;

public class LightFlick : MonoBehaviour
{
    [SerializeField] Animator darkAnimator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip flick;
    private bool triggered = false;
    [SerializeField] private bool useClosing3 = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&& !triggered)
        {
            triggered = true;
            darkAnimator.gameObject.SetActive(true);
            if (useClosing3) darkAnimator.SetTrigger("Closing3");
            else darkAnimator.SetTrigger("Closing2");
            audioSource.PlayOneShot(flick);
        }
    }

}