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
    private Coroutine musicFadeRoutine;
    private Coroutine ambientFadeRoutine;

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
        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);
    
        if (musicSource != null && musicSource.isPlaying)
        {
            musicFadeRoutine = StartCoroutine(
                FadeOutRoutine(musicSource, () => musicFadeRoutine = null)
            );        
        }
    }
    public void StopAmbientSource()
    {
        if (ambientSource != null && ambientSource.isPlaying)
        {
            StartCoroutine(
                FadeOutRoutine(ambientSource, () => ambientFadeRoutine = null)
            );
        }
    }
    public void ResumeAmbientSource()
    {
        ambientSource.volume = 0f;
        ambientSource.Play();
        ambientFadeRoutine = StartCoroutine(
            FadeInRoutine(() => ambientFadeRoutine = null)
        );
    }
    private IEnumerator FadeInRoutine(System.Action onComplete = null)
    {
        float startVolume = ambientSource.volume;
        float timer = 0f;
    
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            ambientSource.volume = Mathf.Lerp(startVolume, 1f, timer / fadeDuration);
            yield return null;
        }
        ambientSource.volume = 1f;
        onComplete?.Invoke();
    }

    private IEnumerator FadeOutRoutine(AudioSource source, System.Action onComplete = null)
    {
        if (source == null)
            yield break;

        float startVolume = source.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
        source.volume = startVolume;

        onComplete?.Invoke();
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
            yield return new WaitForSeconds(Random.Range(10, 100));
        }
    }
    
    public void GameStart()
    {
        sfxSource.PlayOneShot(gameStartSound);
    }
}