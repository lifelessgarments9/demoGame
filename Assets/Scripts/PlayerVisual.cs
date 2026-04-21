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

        if (moving)
        {
            if (dir.x > 0 && dir.y > 0)
                newState = "WalkUpRight";      // W + D
            else if (dir.x < 0 && dir.y > 0)
                newState = "WalkUpLeft";       // W + A
            else if (dir.x > 0 && dir.y < 0)
                newState = "WalkDownRight";    // S + D
            else if (dir.x < 0 && dir.y < 0)
                newState = "WalkDownLeft";     // S + A

            else if (dir.x > 0)
                newState = "WalkRight";    //d
            else if (dir.x < 0)
                newState = "WalkLeft";  //a
            else if (dir.y > 0)
                newState = "WalkUp";   //w
            else
                newState = "WalkDown";  //s
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
            return "IdleUpRight";      // W + D
        else if (dir.x < 0 && dir.y > 0)
            return "IdleUpLeft";       // W + A
        else if (dir.x > 0 && dir.y < 0)
            return "IdleDownRight";    // S + D
        else if (dir.x < 0 && dir.y < 0)
            return "IdleDownLeft";     // S + A

        else if (dir.x > 0)
            return "IdleRight";    //d
        else if (dir.x < 0)
            return "IdleLeft";  //a
        else if (dir.y > 0)
            return "IdleUp";   //w
        else
            return "IdleDown";  //s
    }

}
