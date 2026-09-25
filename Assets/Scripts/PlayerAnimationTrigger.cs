using System.Collections;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private string animationName;
    [SerializeField] private float duration = 3f;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        player.inputLocked = true;

        playerVisual.PlayAnimation(animationName);

        yield return new WaitForSeconds(duration);

        player.inputLocked = false;
    }
}