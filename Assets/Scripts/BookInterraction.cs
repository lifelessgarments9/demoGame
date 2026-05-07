using UnityEngine;
using UnityEngine.InputSystem;

public class BookInteraction : MonoBehaviour
{
    [SerializeField] private HerbariumController herbariumController;  
    [SerializeField] private Player player;
    [SerializeField] private int pawIndex;

    private bool playerInside = false;

    void Update()
    {
        if (!playerInside) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            herbariumController.OpenBook(pawIndex);        
            player.inputLocked = true;             
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