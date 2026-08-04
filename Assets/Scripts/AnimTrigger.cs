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
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing");

        yield return new WaitForSeconds(showdelay);
        
        player.inputLocked = true;
        videoPlayer.clip = clip;
        videoPlayer.Play();
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        
        videoPlayer.clip = null;
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Opening");
        player.inputLocked = false;
    }
}
