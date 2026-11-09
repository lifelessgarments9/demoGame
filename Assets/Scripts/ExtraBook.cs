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
    
    private ImagePanController imagePan;
    
    private Coroutine routine;
    private bool canClose;
    private GameObject currentImage;
    
    public bool OpenBook(GameObject image, string startAnimName = null,bool viewmode=false)
    {
        if (routine != null) return false;
        currentImage = image;
        InputManager.IsInUI = true;
        routine = StartCoroutine(BookRoutine(startAnimName,viewmode));
        return true;
    }
    public void OpenView(GameObject image)
    {
        if (routine != null) return;
        currentImage = image;
        InputManager.IsInUI = true;
        routine = StartCoroutine(BookRoutine(null,true));
    }

    IEnumerator BookRoutine(string startAnimName,bool viewmode)
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
        
        imagePan = currentImage.GetComponentInChildren<ImagePanController>(true);
        if (imagePan != null) imagePan.SetViewMode(viewmode);
        
        closeButton.SetActive(true);
        if(!viewmode) vignetteAnimator.SetActive(false);
        if (!string.IsNullOrEmpty(startAnimName))
        {
            Animator anim = currentImage.GetComponent<Animator>();
            if (anim != null) anim.Play(startAnimName, 0, 0f);
        }
        yield return new WaitForSeconds(showTime);
        
        yield return StartCoroutine(FadeInCloseButton());
        canClose = true;
    }
    
    IEnumerator FadeInCloseButton()
    {
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
        canClose = false;
        StartCoroutine(CloseBookRoutine());
    }

    IEnumerator CloseBookRoutine()
    {
        closeButtonCanvasGroup.interactable = false;
        darkAnimator.gameObject.SetActive(true);
        herbariumController.darkAnimationFinished = false;
        darkAnimator.SetTrigger("Closing2");

        yield return new WaitUntil(() => herbariumController.darkAnimationFinished);
        
        if (imagePan != null)
        {
            imagePan.SetViewMode(false);
            imagePan = null;
        }

        vignetteAnimator.SetActive(true);
        currentImage.SetActive(false);
        closeButton.SetActive(false);
        player.inputLocked = false;
        routine = null;
        currentImage = null;
        herbariumController.UpdatePlayerAnimationStage();
        InputManager.IsInUI = false;
    }
}