using System.Collections;
using UnityEngine;

public class ExtraBook : MonoBehaviour
{

    [SerializeField] private Animator darkAnimator;
    [SerializeField] private Player player;

    [SerializeField] private float transitionDelay = 0.7f;
    [SerializeField] private float showTime = 6f;
    
    [SerializeField] GameObject vignetteAnimator;
    
    [SerializeField] private HerbariumController herbariumController;

    private Coroutine routine;
    
    public void OpenBook(GameObject image)
    {
        if (routine != null) return;
        routine = StartCoroutine(BookRoutine(image));
    }

    IEnumerator BookRoutine(GameObject image)
    {
        darkAnimator.gameObject.SetActive(true);
        herbariumController.darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing");
        
        yield return new WaitUntil(() => herbariumController.darkAnimationFinished);
        
        player.inputLocked = true;
        image.SetActive(true);
        vignetteAnimator.SetActive(false);
        
        yield return new WaitForSeconds(showTime);
        
        darkAnimator.gameObject.SetActive(true);
        herbariumController.darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing2");

        yield return new WaitUntil(() => herbariumController.darkAnimationFinished);
        
        vignetteAnimator.SetActive(true);
        image.SetActive(false);
        player.inputLocked = false;
        routine = null;
    }
}