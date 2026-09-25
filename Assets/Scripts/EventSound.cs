using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class EventSound : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    [SerializeField] private bool entity;
    [SerializeField] private MindTrigger mindTrigger;
    [SerializeField] private GameObject ruThought;
    [SerializeField] private GameObject engThought;
    [SerializeField] private float delay = 4f;
    private AudioSource audioSource;
    public static bool thoughtShown;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (entity && !thoughtShown)
            {
                thoughtShown=true;
                StartCoroutine(ThoughtDelay());
            }
            audioSource.PlayOneShot(sound);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator ThoughtDelay()
    {
        yield return new WaitForSeconds(delay);
        var image = GameFlow.IsRussian ? ruThought : engThought;
        StartCoroutine(mindTrigger.ShowThought(image, 5f,1.5f));
    }
}