using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject UIMenu;
    [SerializeField] Player player;
    [SerializeField] float hidemenudelay=0.7f;
    private Animator animator;
    private GameObject go;
    private CanvasGroup cg;
    private GameState currentState;
    
    public enum GameState
    {
        Menu,
        Game
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        cg=GetComponent<CanvasGroup>();
        //go = GetComponent<GameObject>();
    }
    void Start()
    {
        SetGameState(GameState.Menu);
    }

    void Update()
    {
        
        if (Keyboard.current.enterKey.wasPressedThisFrame && currentState==GameState.Menu)
        {
            SetGameState(GameState.Game);
            //OnStartButton();
        }
        //---------del before production
        if (Keyboard.current.pKey.wasPressedThisFrame && currentState == GameState.Game)
        {
            SetGameState(GameState.Menu);
        }
        //if(Keyboard.current.enterKey.wasPressedThisFrame||Keyboard.current.spaceKey.wasPressedThisFrame)
    }

    public void OnStartButton()
    {
        SetGameState(GameState.Game);
    }

    public void OnAnimationFinished()
    {
        //cg.alpha = 0;
        
        animator.gameObject.SetActive(false);
    }

    void SetGameState(GameState state)
    {
        currentState=state;
        
        //animator.ResetTrigger("Opening");
        //animator.ResetTrigger("Closing");
        
        animator.gameObject.SetActive(true);
        //cg.alpha = 1;
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
