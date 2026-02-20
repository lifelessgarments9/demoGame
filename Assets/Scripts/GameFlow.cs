using UnityEngine.InputSystem;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject UIMenu;
    [SerializeField] Player player;
    private Animator animator;
    
    public enum GameState
    {
        Menu,
        Game
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        animator.gameObject.SetActive(false);
        SetGameState(GameState.Menu);
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            SetGameState(GameState.Game);
            //OnStartButton();
        }
    }

    public void OnStartButton()
    {
        SetGameState(GameState.Game);
    }
    public void OnOpeningFinished()
    {
        animator.gameObject.SetActive(false);
    }

    void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                UIMenu.SetActive(true);
                player.inputLocked = true;
                break;
            case GameState.Game:
                UIMenu.SetActive(false);
                animator.gameObject.SetActive(true);
                player.inputLocked = false;
                animator.SetTrigger("Opening");
                break;
        }
    }
}
