using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    
    private Animator animator;
    string currentState;
    string idleState;
    bool wasMoving;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Vector2 dir = Player.Instance.LastMoveDir;
        bool moving = Player.Instance.IsMoving;

        if (!moving && wasMoving) //if 2f idle  - wasMoving constr, if moving - !moving constr => only first frame of idle
        {
            idleState = PickIdle(dir);
        }

        wasMoving = moving;
        
        string newState;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
                newState = moving ? "WalkRight" : idleState;
            else
                newState = moving ? "WalkLeft" : idleState;
        }
        else
        {
            if (dir.y > 0)
                newState = moving ? "WalkUp" : idleState;
            else
                newState = moving ? "WalkDown" : idleState;
        }
        

        if (currentState != newState)
        {
            animator.Play(newState);
            currentState = newState;
        }
    }
    
    string PickIdle(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
                return Random.value < 0.85f ? "IdleRight1" : "IdleRight";
            else
                return Random.value < 0.85f ? "IdleLeft1" : "IdleLeft";
        }
        else
        {
            if (dir.y > 0)
                return Random.value < 0.85f ? "IdleUp1" : "IdleUp";
            else
                return Random.value < 0.85f ? "IdleDown1" : "IdleDown";
        }
    }

}
