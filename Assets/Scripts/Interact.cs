using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField] GameObject uiImg;
    [SerializeField] Player player;
    [SerializeField] bool grabbable= false;
    private bool playerInside = false;
    private bool uiOpen = false;
    private bool used = false;

    void Update()
    {
        if(!playerInside) return;
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleUI();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) playerInside=true;
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) playerInside=false;
    }

    void ToggleUI()
    {
        if (grabbable && used && !uiOpen)return;
        
        uiOpen = !uiOpen;
        uiImg.SetActive(uiOpen);
        player.inputLocked = uiOpen;

        if (grabbable && uiOpen)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            used = true;
        }
    }
}
