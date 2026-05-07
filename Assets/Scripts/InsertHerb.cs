
using UnityEngine;
using UnityEngine.EventSystems;

public class InsertHerb : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private HerbariumController controller;

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.TryQuitFromClick();
    }
}
