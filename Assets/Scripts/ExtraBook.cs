using System.Collections;
using UnityEngine;

public class ExtraBook : MonoBehaviour
{

    [SerializeField] private Animator darkAnimator;
    [SerializeField] private Player player;
    [SerializeField] private float showTime = 6f;
    [SerializeField] GameObject vignetteAnimator;
    [SerializeField] private HerbariumController herbariumController;
    
    [SerializeField] private GameObject closeButton;
    [SerializeField] private CanvasGroup closeButtonCanvasGroup;
    [SerializeField] private float buttonFadeDuration = 1f;
    
    private Coroutine routine;
    private bool canClose;
    private GameObject currentImage;
    
    public void OpenBook(GameObject image)
    {
        if (routine != null) return;
        currentImage = image;
        routine = StartCoroutine(BookRoutine());
    }

    IEnumerator BookRoutine()
    {
        canClose = false;
        closeButtonCanvasGroup.alpha = 0f;
        closeButtonCanvasGroup.interactable = false;
        
        darkAnimator.gameObject.SetActive(true);
        herbariumController.darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing");
        
        yield return new WaitUntil(() => herbariumController.darkAnimationFinished);
        
        player.inputLocked = true;
        currentImage.SetActive(true);
        closeButton.SetActive(true);
        vignetteAnimator.SetActive(false);
        
        yield return new WaitForSeconds(showTime);
        
        yield return StartCoroutine(FadeInCloseButton());
        canClose = true;
    }
    
    IEnumerator FadeInCloseButton()
    {
        closeButtonCanvasGroup.blocksRaycasts = true;

        float timer = 0f;

        while (timer < buttonFadeDuration)
        {
            timer += Time.deltaTime;

            closeButtonCanvasGroup.alpha =
                Mathf.Clamp01(timer / buttonFadeDuration);

            yield return null;
        }

        closeButtonCanvasGroup.alpha = 1f;
        closeButtonCanvasGroup.interactable = true;
    }
    
    public void CloseBook()
    {
        if (!canClose || routine == null)
            return;

        StartCoroutine(CloseBookRoutine());
    }

    IEnumerator CloseBookRoutine()
    {
        canClose = false;

        closeButtonCanvasGroup.interactable = false;

        darkAnimator.gameObject.SetActive(true);
        herbariumController.darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing2");

        yield return new WaitUntil(() => herbariumController.darkAnimationFinished);

        vignetteAnimator.SetActive(true);
        currentImage.SetActive(false);
        closeButton.SetActive(false);
        player.inputLocked = false;
        routine = null;
        currentImage = null;
        herbariumController.UpdatePlayerAnimationStage();
    }
}