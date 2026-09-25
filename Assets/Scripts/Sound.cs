using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class Sound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (audioSource.isPlaying) audioSource.Stop();
            audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
        }
    }
}