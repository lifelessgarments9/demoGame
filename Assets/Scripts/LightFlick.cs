using System.Collections.Generic;
using UnityEngine;
public class LightFlick : MonoBehaviour
{
    [SerializeField] Animator darkAnimator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip flick;
    private bool triggered = false;
    [SerializeField] private List<string> animNames;
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
            darkAnimator.SetTrigger(animNames[Random.Range(0, animNames.Count)]);
            audioSource.PlayOneShot(flick);
        }
    }
}