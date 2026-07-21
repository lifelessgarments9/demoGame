using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> thoughtImgs;
    [SerializeField] private List<GameObject> engThoughtImgs;

    [SerializeField] private float ShowTime = 5f;
    [SerializeField] private float FadeTime = 2f;

    private bool triggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        StartCoroutine(ThoughtChain());
    }

    IEnumerator ThoughtChain()
    {
        var images = GameFlow.IsRussian ? thoughtImgs : engThoughtImgs;
        foreach (var img in images)
        {
            if (img != null)
            {
                yield return ShowThought(img);
            }
        }
    }

    public IEnumerator ShowThought(GameObject img)
    {
        CanvasGroup cg = img.GetComponent<CanvasGroup>();

        if (cg == null)
        {
            Debug.LogWarning($"No CanvasGroup on {img.name}");
            yield break;
        }

        img.SetActive(true);

        float t = 0f;

        cg.alpha = 0f;

        while (t < FadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = t / FadeTime;
            yield return null;
        }

        cg.alpha = 1f;

        yield return new WaitForSeconds(ShowTime);

        t = 0f;

        while (t < FadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = 1f - t / FadeTime;
            yield return null;
        }

        cg.alpha = 0f;
        img.SetActive(false);
    }
}