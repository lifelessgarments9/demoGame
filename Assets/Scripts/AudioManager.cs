using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] AudioClip dasha;
    [SerializeField] AudioClip engDasha; 
    [SerializeField] AudioClip gameStartSound;
    [SerializeField] AudioClip crickets;
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> rareSounds = new List<AudioClip>();
    [SerializeField] private float fadeDuration = 1.5f;
    AudioClip clip;
    private Coroutine musicRoutine;

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
    public void BackgroundMusic()
    {
        musicRoutine=StartCoroutine(MusicLoop());
    }
    public void StopBackgroundMusic()
    {
        if (musicRoutine != null)
        {
            StopCoroutine(musicRoutine);
            musicRoutine = null;
        }
    
        if (musicSource != null && musicSource.isPlaying)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float startVolume = musicSource.volume;
        float timer = 0f;
    
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; 
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }
    
        musicSource.volume = 0f;
        musicSource.Stop();
        musicSource.volume = startVolume; 
    }

    private IEnumerator MusicLoop()
    {
        while (true)
        {
            if (rareSounds.Count > 0 && Random.Range(0f, 100f) <= 7f)
            {
                clip = rareSounds[Random.Range(0, rareSounds.Count)];
            }
            else
            {
                clip = sounds[Random.Range(0, sounds.Count)];
            }
            musicSource.clip = clip;
            musicSource.Play();
            yield return new WaitWhile(() => musicSource.isPlaying);
            yield return new WaitForSeconds(Random.Range(1, 90));
        }
    }
    
    public void GameStart()
    {
        sfxSource.PlayOneShot(gameStartSound);
    }
}