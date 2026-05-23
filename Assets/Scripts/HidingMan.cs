using UnityEngine;

public class HidingMan : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("MoveLeft");
        }
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}