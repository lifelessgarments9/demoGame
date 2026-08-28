using UnityEngine;
using UnityEngine.Video;
using System.Collections;
public class AnimTrigger : MonoBehaviour
{
    [SerializeField] Animator darkAnimator;
    private bool triggered;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] private VideoClip clip;
    [SerializeField] private float showdelay = 0.7f;
    [SerializeField] private Player player;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&& !triggered)
        {
            triggered = true;
            StartCoroutine(ShowVideo());
        }
    }
    
    IEnumerator ShowVideo()
    {
        audioSource.Play();
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing");

        yield return new WaitForSeconds(showdelay);
        
        player.inputLocked = true;
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.clip = clip;
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        videoPlayer.Stop();
        videoPlayer.clip = null;
        darkAnimator.gameObject.SetActive(true);
        videoPlayer.gameObject.SetActive(false);
        darkAnimator.SetTrigger("Opening");
        player.inputLocked = false;
    }
}
