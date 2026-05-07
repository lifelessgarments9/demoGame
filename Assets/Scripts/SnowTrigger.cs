using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SnowTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject snow;
    [SerializeField] private float delayForSnowflake;
    private bool inside = false;
    private Coroutine snowRoutine;
    private int snowflakeIndex = 1;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inside = true;
            snow.SetActive(true);
            if (snowRoutine != null)
            {
                StopCoroutine(snowRoutine);
            }
            snowRoutine = StartCoroutine(SnowSequence());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inside = false;
        }
    }

    IEnumerator SnowSequence()
    {
        animator.Play("snowflake" + snowflakeIndex);
        snowflakeIndex++;
        if (snowflakeIndex > 5) snowflakeIndex = 1;
        yield return new WaitForSeconds(delayForSnowflake);
        while (inside)
        {
            animator.Play("snow1");
            yield return WaitAnimation();
            if (!inside)
            {
                StopSnow();
                yield break;
            }
            animator.Play("snow2");
            yield return WaitAnimation();
            if (!inside)
            {
                StopSnow();
                yield break;
            }
        }
    }
    void StopSnow()
    {
        if (snow)
        {
            snow.SetActive(false);
        }
        snowRoutine = null;
    }
    IEnumerator WaitAnimation()
    {
        yield return null; //ждем кадр для записи текущего состояния анимации 
        var state = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(state.length);
    }
}