using UnityEngine;
public class UIAnimationEvents : MonoBehaviour
{
    [SerializeField] GameFlow gameFlow;

    public void OnAnimationFinished()
    {
        gameFlow.OnAnimationFinished();
    }
}