using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip dasha; 
    [SerializeField] AudioClip gameStartSound; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        sfxSource.Play();
        musicSource.Play();
    }
    public void Dasha()
    {
        sfxSource.PlayOneShot(dasha);
    }
    
    public void GameStart()
    {
        sfxSource.PlayOneShot(gameStartSound);
    }
}