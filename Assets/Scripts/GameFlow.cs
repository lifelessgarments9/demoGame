using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject UIMenu;
    [SerializeField] private GameObject engPlayButton;
    [SerializeField] private GameObject ruPlayButton;
    [SerializeField] private GameObject engLangButton;
    [SerializeField] private GameObject ruLangButton;
    [SerializeField] Player player;
    [SerializeField] float hidemenudelay=0.7f;
    [SerializeField] float dashaDelay=3f;
    [SerializeField] Animator animator;
    [Header("Thought Chain")]
    [SerializeField] MindTrigger mindTrigger;
    [SerializeField] List<GameObject> thoughtImgs;
    [SerializeField] List<GameObject> engThoughtImgs;
    [SerializeField] GameObject mindTriggerGroup;
    
    private GameObject go;
    private GameState currentState;
    public GameState CurState => currentState;
    
    public enum GameState {Menu, Game}

    public static bool IsRussian { get; private set; } = true;
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
    
    public void OnLangButton()
    {
        IsRussian = !IsRussian;
        UpdateLanguageUI();
    }

    private void UpdateLanguageUI()
    {
        engPlayButton.SetActive(!IsRussian);
        engLangButton.SetActive(!IsRussian);
        ruPlayButton.SetActive(IsRussian);
        ruLangButton.SetActive(IsRussian);
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
                UpdateLanguageUI();
                player.inputLocked = true;
                animator.SetTrigger("Opening");
                break;
            case GameState.Game:
                player.inputLocked = false;
                animator.SetTrigger("Closing");
                AudioManager.Instance.GameStart();
                StartCoroutine(HideMenu(hidemenudelay));
                StartCoroutine(PlayDasha(dashaDelay));
                break;
        }
    }

    IEnumerator HideMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIMenu.SetActive(false);
    }
    IEnumerator PlayDasha(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.Dasha();
        yield return new WaitForSeconds(32f);
        
        var images = IsRussian ? thoughtImgs : engThoughtImgs;
        foreach (var img in images)
        {
            yield return StartCoroutine(mindTrigger.ShowThought(img));
        }
        
        if (mindTriggerGroup != null)
        {
            mindTriggerGroup.SetActive(true);
        }
    }
}
