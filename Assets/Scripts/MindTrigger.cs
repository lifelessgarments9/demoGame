using System.Collections;
using UnityEngine;

public class MindTrigger : MonoBehaviour
{
    [SerializeField] GameObject thoughtImg;
    [SerializeField] private float ShowTime = 5f;
    [SerializeField] private float FadeTime = 2f;
    private bool triggered=false;
    private CanvasGroup cg;

    void Awake()
    {
        cg = thoughtImg.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("no CanvasGroup");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(Thought());
        }

    }

    IEnumerator Thought()
    {
        thoughtImg.SetActive(true);
        
        cg.alpha = 0f;
        float t = 0f;
        while (t < FadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = t/FadeTime;
            yield return null;
        }
        
        cg.alpha = 1f;
        yield return new WaitForSeconds(ShowTime);
        
        t = 0f;
        while (t < FadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = 1f-(t/FadeTime);
            yield return null;
        }

        cg.alpha = 0f;
        thoughtImg.SetActive(false);
    }
    
}
