using UnityEngine;
public class UIAnimationEvents : MonoBehaviour
{
    [SerializeField] GameFlow gameFlow;
    [SerializeField] HerbariumController herbariumController;

    public void OnAnimationFinished()
    {
        gameFlow.OnAnimationFinished();
    }
    public void OnDark()
    {
        herbariumController.OnDark();
    }
}