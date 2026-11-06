using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class EventSound : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    [SerializeField] private bool entity;
    [SerializeField] private MindTrigger mindTrigger;
    [SerializeField] private GameObject ruThought;
    [SerializeField] private GameObject engThought;
    [SerializeField] private GameObject timer;
    [SerializeField] private float delay = 4f;
    [SerializeField] private RectTransform timerArrow;
    [SerializeField] private float rotationSpeed = 90f;
    private AudioSource audioSource;
    private Collider2D myCollider;
    public static bool thoughtShown;
    private bool isUsed = false;
    
    [SerializeField] private List<EventSound> eventSounds;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        myCollider = GetComponent<Collider2D>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed) return;
        if (other.gameObject.CompareTag("Player"))
        {
            isUsed = true;
            myCollider.enabled = false;
            if (entity)
            {
                StartCoroutine(StartTimer());
                if (!thoughtShown)
                {
                    thoughtShown = true;
                    StartCoroutine(ThoughtDelay());
                }
            }
            audioSource.PlayOneShot(sound);
        }
    }
    private IEnumerator StartTimer()
    {
        SetOtherEventsBlocked(true);
        timer.gameObject.SetActive(true);
        
        timerArrow.localRotation = Quaternion.identity;
        float elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += Time.unscaledDeltaTime;
            float currentAngle = elapsed * rotationSpeed;
            timerArrow.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
            yield return null;
        }
        timerArrow.localRotation = Quaternion.identity;
        
        timer.gameObject.SetActive(false);
        SetOtherEventsBlocked(false);
    }
    private void SetOtherEventsBlocked(bool blocked)
    {
        foreach (EventSound eventSound in eventSounds)
        {
            if (eventSound == null || eventSound == this) continue;
            if (!eventSound.isUsed) eventSound.myCollider.enabled = !blocked;
        }
    }

    IEnumerator ThoughtDelay()
    {
        yield return new WaitForSeconds(delay);
        var image = GameFlow.IsRussian ? ruThought : engThought;
        StartCoroutine(mindTrigger.ShowThought(image, 5f,1.5f));
    }
}