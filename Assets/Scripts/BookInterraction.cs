using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    [SerializeField] private HerbariumController herbariumController;
    [SerializeField] private ExtraBook extraBook;
    [SerializeField] private Player player;
    [SerializeField] private int pawIndex;
    [SerializeField] private GameObject image;

    private bool used;

    void Interact()
    {
        if (used)
            return;

        used = true;

        if (extraBook == null)
        {
            herbariumController.OpenBook(pawIndex);
            player.inputLocked = true;
        }
        else
        {
            extraBook.OpenBook(image);
        }

        InputManager.Instance.InteractPressed -= Interact;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.Instance.InteractPressed += Interact;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.Instance.InteractPressed -= Interact;
        }
    }
}