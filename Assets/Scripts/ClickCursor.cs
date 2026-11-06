using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCursor : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (used) return;
        CustomCursor.SetClickCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (used) return;
        CustomCursor.SetNormalCursor();
    }
}