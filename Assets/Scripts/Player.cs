using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Vector2 LastMoveDir { get; private set; }
    public bool IsMoving { get; private set; }
    private AudioSource footsteps;
    [SerializeField] float speed = 5f;
    private Rigidbody2D rb;
    public bool inputLocked = false;
    bool footstepPlaying=false;
    private Hill hill;
    [SerializeField] private List<AudioClip> footstepClips = new List<AudioClip>();
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        footsteps=GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (inputLocked)
        {
            if (footstepPlaying)
            {
                footsteps.Stop();
                footstepPlaying = false;
            }
            if (rb.linearVelocity != Vector2.zero)
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }
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
            if (hill != null) vector = hill.ModifyDirection(rb.position, vector);
            LastMoveDir = vector;
            rb.MovePosition(rb.position + vector * speed * Time.fixedDeltaTime);
            if (!footstepPlaying)
            {
                footsteps.clip = footstepClips[Random.Range(0, footstepClips.Count)];
                footsteps.Play();
                footstepPlaying = true;
            }
        }
        else
        {
            if (footstepPlaying)
            {
                footsteps.Stop();
                footstepPlaying = false;
            }
        }
        
    }
    public void SetHill(Hill h)
    {
        hill = h;
    }

    public void ClearHill(Hill h)
    {
        if (hill == h) hill = null;
    }
}