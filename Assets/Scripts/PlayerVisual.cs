using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private string currentState;
    private string idleState;
    private bool wasMoving;

    // ===== WALK =====
    private string walkUpRight = "WalkUpRight";
    private string walkUpLeft = "WalkUpLeft";
    private string walkDownRight = "WalkDownRight";
    private string walkDownLeft = "WalkDownLeft";

    private string walkRight = "WalkRight";
    private string walkLeft = "WalkLeft";
    private string walkUp = "WalkUp";
    private string walkDown = "WalkDown";

    // ===== IDLE =====
    private string idleUpRight = "IdleUpRight";
    private string idleUpLeft = "IdleUpLeft";
    private string idleDownRight = "IdleDownRight";
    private string idleDownLeft = "IdleDownLeft";

    private string idleRight = "IdleRight";
    private string idleLeft = "IdleLeft";
    private string idleUp = "IdleUp";
    private string idleDown = "IdleDown";


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 dir = Player.Instance.LastMoveDir;
        bool moving = Player.Instance.IsMoving;

        if (!moving && wasMoving)
        {
            idleState = PickIdle(dir);
        }

        wasMoving = moving;

        string newState;

        if (moving)
        {
            if (dir.x > 0 && dir.y > 0)
                newState = walkUpRight;

            else if (dir.x < 0 && dir.y > 0)
                newState = walkUpLeft;

            else if (dir.x > 0 && dir.y < 0)
                newState = walkDownRight;

            else if (dir.x < 0 && dir.y < 0)
                newState = walkDownLeft;

            else if (dir.x > 0)
                newState = walkRight;

            else if (dir.x < 0)
                newState = walkLeft;

            else if (dir.y > 0)
                newState = walkUp;

            else
                newState = walkDown;
        }
        else
        {
            newState = idleState;
        }

        if (currentState != newState)
        {
            animator.Play(newState);
            currentState = newState;
        }
    }


    string PickIdle(Vector2 dir)
    {
        if (dir.x > 0 && dir.y > 0)
            return idleUpRight;

        else if (dir.x < 0 && dir.y > 0)
            return idleUpLeft;

        else if (dir.x > 0 && dir.y < 0)
            return idleDownRight;

        else if (dir.x < 0 && dir.y < 0)
            return idleDownLeft;

        else if (dir.x > 0)
            return idleRight;

        else if (dir.x < 0)
            return idleLeft;

        else if (dir.y > 0)
            return idleUp;

        else
            return idleDown;
    }

    public void SetFirstAnimationSet()
    {
        // ===== WALK =====
        walkUpRight = "WalkUpRight";
        walkUpLeft = "WalkUpLeft";
        walkDownRight = "WalkDownRight";
        walkDownLeft = "WalkDownLeft";

        walkRight = "WalkRight";
        walkLeft = "WalkLeft";
        walkUp = "WalkUp";
        walkDown = "WalkDown";

        // ===== IDLE =====
        idleUpRight = "IdleUpRight";
        idleUpLeft = "IdleUpLeft";
        idleDownRight = "IdleDownRight";
        idleDownLeft = "IdleDownLeft";

        idleRight = "IdleRight";
        idleLeft = "IdleLeft";
        idleUp = "IdleUp";
        idleDown = "IdleDown";
        
        currentState = null;
    }
    public void SetSecondAnimationSet()
    {
        // ===== WALK =====
        walkUpRight = "WalkUpRight2";
        walkUpLeft = "WalkUpLeft2";
        walkDownRight = "WalkDownRight2";
        walkDownLeft = "WalkDownLeft2";

        walkRight = "WalkRight2";
        walkLeft = "WalkLeft2";
        walkUp = "WalkUp2";
        walkDown = "WalkDown2";

        // ===== IDLE =====
        idleUpRight = "IdleUpRight2";
        idleUpLeft = "IdleUpLeft2";
        idleDownRight = "IdleDownRight2";
        idleDownLeft = "IdleDownLeft2";

        idleRight = "IdleRight2";
        idleLeft = "IdleLeft2";
        idleUp = "IdleUp2";
        idleDown = "IdleDown2";
        
        currentState = null;
    }
}