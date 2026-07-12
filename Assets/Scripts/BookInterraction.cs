using UnityEngine;
using UnityEngine.InputSystem;

public class BookInteraction : MonoBehaviour
{
    [SerializeField] private HerbariumController herbariumController; 
    [SerializeField] private ExtraBook extraBook; 
    [SerializeField] private Player player;
    [SerializeField] private int pawIndex;
    [SerializeField] private GameObject image;
    private bool playerInside = false;
    private bool used = false;

    void Update()
    {
        if (!playerInside || used) return;

        if (Keyboard.current.eKey.wasPressedThisFrame&&extraBook == null)
        {
            herbariumController.OpenBook(pawIndex);
            player.inputLocked = true; 
            used = true;
        }
        if (Keyboard.current.eKey.wasPressedThisFrame&&extraBook != null)
        {
            extraBook.OpenBook(image);
            used = true;
        }

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}