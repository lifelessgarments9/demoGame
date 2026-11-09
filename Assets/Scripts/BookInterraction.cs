using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BookInteraction : MonoBehaviour
{
    [SerializeField] private HerbariumController herbariumController;
    [SerializeField] private ExtraBook extraBook;
    [SerializeField] private Player player;
    [SerializeField] private int pawIndex;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject eButton;
    [SerializeField] private float cooldown = 8f;
    [SerializeField] private MindTrigger mindTrigger;
    private bool onCooldown;
    private bool used;

    void Interact()
    {
        bool opened;
        if (extraBook == null)
        {
            opened = herbariumController.OpenBook(pawIndex);
            if (opened) player.inputLocked = true;
        }
        else
        {
            opened = extraBook.OpenBook(image);
        }
        
        if (!opened) return;
        used = true;
        InputManager.Instance.InteractPressed -= Interact;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (used) return;
            InputManager.Instance.InteractPressed += Interact;
            if(onCooldown) return;
            StartCoroutine(SetCooldown());
            StartCoroutine(mindTrigger.ShowThought(eButton, 0.5f, 1f));
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.Instance.InteractPressed -= Interact;
        }
    }
    private IEnumerator SetCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
    private void OnDisable()
    {
        if (InputManager.Instance != null) InputManager.Instance.InteractPressed -= Interact;
    }
}