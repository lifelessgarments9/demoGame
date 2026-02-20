using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Vector2 LastMoveDir { get; private set; }
    public bool IsMoving { get; private set; }

    
    [SerializeField] float speed = 5f;
    private Rigidbody2D rb;
    public bool inputLocked = false;
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (inputLocked) return;
        HandleMovement();
    }

    private void HandleMovement()
    {

        Vector2 vector = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)vector.y += 1f;
        if (Keyboard.current.sKey.isPressed) vector.y -= 1f;
        if (Keyboard.current.dKey.isPressed) vector.x += 1f;
        if (Keyboard.current.aKey.isPressed) vector.x -= 1f;
        if (Keyboard.current.upArrowKey.isPressed) vector.y += 1f;
        if (Keyboard.current.downArrowKey.isPressed) vector.y -= 1f;
        if (Keyboard.current.rightArrowKey.isPressed) vector.x += 1f;
        if (Keyboard.current.leftArrowKey.isPressed) vector.x -= 1f;
        
        IsMoving = vector != Vector2.zero;
        if (IsMoving)
        {
            vector.Normalize();
            LastMoveDir = vector;
            rb.MovePosition(rb.position + vector * speed * Time.fixedDeltaTime);
        }
    }
    
}