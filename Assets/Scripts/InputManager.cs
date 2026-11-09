using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event Action EnterPressed;
    public event Action SpacePressed;
    public event Action InteractPressed;
    public event Action BookPressed;
    public event Action LeftPressed;
    public event Action RightPressed;
    public bool IsChain;
    public static bool IsInUI { get; set; } = false;

    
    [SerializeField] private float bookPressCooldown = 3f;
    private float nextBookPressTime= 0f;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
            EnterPressed?.Invoke();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            SpacePressed?.Invoke();

        if (GameFlow.Instance.CurrentState != GameFlow.GameState.Game)
            return;
        
        if (IsChain) return;
        
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            LeftPressed?.Invoke();

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            RightPressed?.Invoke();
        
        if (Keyboard.current.bKey.wasPressedThisFrame &&
            Time.unscaledTime >= nextBookPressTime)
        {
            nextBookPressTime = Time.unscaledTime + bookPressCooldown;
            BookPressed?.Invoke();
        }
        
        if (IsInUI) return; 

        if (Keyboard.current.eKey.wasPressedThisFrame)
            InteractPressed?.Invoke();
        
    }
}