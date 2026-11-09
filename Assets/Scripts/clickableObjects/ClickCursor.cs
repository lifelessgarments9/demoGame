using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCursor : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(InputManager.IsInUI) return;
        CustomCursor.SetClickCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(InputManager.IsInUI) return;
        CustomCursor.SetNormalCursor();
    }
}