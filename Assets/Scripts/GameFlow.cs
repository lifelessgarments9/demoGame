using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject UIMenu;
    [SerializeField] Player player;
    [SerializeField] float hidemenudelay=0.7f;
    [SerializeField] Animator animator;
    private GameObject go;
    private GameState currentState;
    
    public enum GameState {Menu, Game}
    void Start() => SetGameState(GameState.Menu); 

    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && currentState==GameState.Menu)
        {
            OnStartButton();
        }
    }

    public void OnStartButton()
    {
        SetGameState(GameState.Game);
    }

    public void OnAnimationFinished()
    {
        animator.gameObject.SetActive(false);
    }

    void SetGameState(GameState state)
    {
        currentState=state;
        animator.gameObject.SetActive(true);
        switch (state)
        {
            case GameState.Menu:
                UIMenu.SetActive(true);
                player.inputLocked = true;
                animator.SetTrigger("Opening");
                break;
            case GameState.Game:
                player.inputLocked = false;
                animator.SetTrigger("Closing");
                StartCoroutine(HideMenu(hidemenudelay));
                break;
        }
    }

    IEnumerator HideMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIMenu.SetActive(false);
    }

}
