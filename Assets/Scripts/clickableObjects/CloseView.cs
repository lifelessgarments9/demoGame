using UnityEngine;
using UnityEngine.EventSystems;

public class CloseView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ExtraBook extraBook;
    [SerializeField] private GameObject image;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(InputManager.IsInUI) return;
        extraBook.OpenView(image);
    }
}