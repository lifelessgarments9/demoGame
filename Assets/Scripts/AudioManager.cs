using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] AudioClip dasha;
    [SerializeField] AudioClip engDasha; 
    [SerializeField] AudioClip gameStartSound;
    [SerializeField] AudioClip crickets;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ambientSource.loop = true;
        ambientSource.clip = crickets;
        ambientSource.Play();
    }
    public void Dasha()
    {
        if (GameFlow.IsRussian) sfxSource.PlayOneShot(dasha);
        else sfxSource.PlayOneShot(engDasha);
    }
    
    public void GameStart()
    {
        sfxSource.PlayOneShot(gameStartSound);
    }
}