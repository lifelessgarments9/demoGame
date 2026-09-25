using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject UIMenu;
    
    [SerializeField] private GameObject engDesc;
    [SerializeField] private GameObject ruDesc;
    [SerializeField] private GameObject engOkButton;
    [SerializeField] private GameObject ruOkButton;
    
    [SerializeField] private GameObject engPlayButton;
    [SerializeField] private GameObject ruPlayButton;
    [SerializeField] private GameObject engLangButton;
    [SerializeField] private GameObject ruLangButton;
    
    [SerializeField] private GameObject ruExitButton;
    [SerializeField] private GameObject engExitButton;
    
    [SerializeField] private GameObject engResumeButton;
    [SerializeField] private GameObject ruResumeButton;

    [SerializeField] private Player player;
    [SerializeField] private Animator animator;

    [SerializeField] private float hidemenudelay = 0.7f;
    [SerializeField] private float dashaDelay = 3f;

    [Header("Thought Chain")]
    [SerializeField] private MindTrigger mindTrigger;
    [SerializeField] private List<GameObject> thoughtImgs;
    [SerializeField] private List<GameObject> engThoughtImgs;
    [SerializeField] private GameObject mindTriggerGroup;
    private bool isTransition;
    public static bool IsRussian { get; private set; } = false;
    public static bool firstGame = true;
    public enum GameState { Menu, Desc, Game, Pause }
    private GameState currentState;
    public static GameFlow Instance { get; private set; }
    public GameState CurrentState => currentState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InputManager.Instance.EnterPressed += OnEnterPressed;
        InputManager.Instance.SpacePressed += OnSpacePressed;
        
        mindTriggerGroup.SetActive(false);
        SetGameState(GameState.Menu);
        InputManager.Instance.IsChain = true;
    }
    
    void OnEnterPressed()
    {
        switch (currentState)
        {
            case GameState.Menu:
                OnStartButton();
                break;

            case GameState.Desc:
                OnOkButton();
                break;
        }
    }

    void OnSpacePressed()
    {
        switch (currentState)
        {
            case GameState.Game:
                Pause();
                break;

            case GameState.Pause:
                OnResumeButton();
                break;
        }
    }

    void Pause()
    {
        currentState = GameState.Pause;
        player.inputLocked = true;
        UIMenu.SetActive(true);
        ruResumeButton.SetActive(IsRussian);
        engResumeButton.SetActive(!IsRussian);
        engExitButton.SetActive(!IsRussian);
        ruExitButton.SetActive(IsRussian);
    }
    
    public void OnResumeButton()
    {
        currentState = GameState.Game;
        UIMenu.SetActive(false);
        player.inputLocked = false;
    }

    public void OnStartButton()
    {
        if (isTransition)
            return;

        isTransition = true;
        SetGameState(GameState.Desc);
    }

    public void OnOkButton()
    {
        if (isTransition)
            return;

        isTransition = true;
        SetGameState(GameState.Game);
    }

    public void OnLangButton()
    {
        IsRussian = !IsRussian;
        UpdateLanguageUI();
    }
    public void OnExitButton()
    {
        Application.Quit();
    }

    private void UpdateLanguageUI()
    {
        engPlayButton.SetActive(!IsRussian);
        engLangButton.SetActive(!IsRussian);
        engExitButton.SetActive(!IsRussian);
        ruPlayButton.SetActive(IsRussian);
        ruLangButton.SetActive(IsRussian);
        ruExitButton.SetActive(IsRussian);
    }
    private void AllOff()
    {
        engExitButton.SetActive(false);
        ruExitButton.SetActive(false);
        engPlayButton.SetActive(false);
        engLangButton.SetActive(false);
        ruPlayButton.SetActive(false);
        ruLangButton.SetActive(false);
        engDesc.SetActive(false);
        ruDesc.SetActive(false);
        engOkButton.SetActive(false);
        ruOkButton.SetActive(false);
        engResumeButton.SetActive(false);
        ruResumeButton.SetActive(false);
    }

    public void OnAnimationFinished()
    {
        animator.gameObject.SetActive(false);
    }

    void SetGameState(GameState state)
    {
        currentState = state;
        switch (state)
        {
            case GameState.Menu:
                isTransition = true;
                animator.gameObject.SetActive(true);
                UIMenu.SetActive(true);
                UpdateLanguageUI();
                player.inputLocked = true;
                animator.SetTrigger("Opening2");
                StartCoroutine(Opening2Wait());
                ChunkActivationSystem.Instance.InitializeChunks();
                break;

            case GameState.Desc:
                StartCoroutine(OpenDescription());
                break;

            case GameState.Game:
                StartCoroutine(StartGame());
                break;
        }
    }
    
    IEnumerator Opening2Wait()
    {
        yield return new WaitForSeconds(2f);
        isTransition = false;
    }

    IEnumerator OpenDescription()
    {
        animator.gameObject.SetActive(true);
        animator.SetTrigger("Closing");

        yield return new WaitForSeconds(hidemenudelay);
        
        AllOff();
        ruDesc.SetActive(IsRussian);
        ruOkButton.SetActive(IsRussian);
        engDesc.SetActive(!IsRussian);
        engOkButton.SetActive(!IsRussian);
        
        yield return new WaitForSeconds(hidemenudelay);
        
        isTransition = false;
    }

    IEnumerator StartGame()
    {
        animator.gameObject.SetActive(true);
        animator.SetTrigger("Closing");

        yield return new WaitForSeconds(hidemenudelay);

        AllOff();
        UIMenu.SetActive(false);
        player.inputLocked = false;
        AudioManager.Instance.GameStart();
        
        if(firstGame){StartCoroutine(PlayDasha());}
        else{mindTriggerGroup.SetActive(true);}
        
        isTransition = false;
    }

    IEnumerator PlayDasha()
    {
        yield return new WaitForSeconds(dashaDelay);

        AudioManager.Instance.Dasha();

        yield return new WaitForSeconds(30f);

        var images = IsRussian ? thoughtImgs : engThoughtImgs;
        foreach (var img in images)
        {
            yield return StartCoroutine(mindTrigger.ShowThought(img, 5f,1.5f));
        }
        InputManager.Instance.IsChain = false;
        mindTriggerGroup.SetActive(true);
    }
}