using System.Collections;
using UnityEngine;

public class ExtraBook : MonoBehaviour
{

    [SerializeField] private Animator darkAnimator;
    [SerializeField] private Player player;

    [SerializeField] private float transitionDelay = 0.7f;
    [SerializeField] private float showTime = 6f;

    private Coroutine routine;


    public void OpenBook(GameObject image)
    {
        if (routine != null)
            return;

        routine = StartCoroutine(BookRoutine(image));
    }

    IEnumerator BookRoutine(GameObject image)
    {
        
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing");

        yield return new WaitForSeconds(transitionDelay);


        // показ книги
        yield return new WaitForSeconds(showTime);

        // затемнение перед закрытием
        darkAnimator.gameObject.SetActive(true);
        darkAnimator.SetTrigger("Closing2");

        yield return new WaitForSeconds(transitionDelay);


        player.inputLocked = false;
        routine = null;
    }
}