using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerUIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized] public ItemUI itemUI;

    private void Awake()
    {
        itemUI = GetComponent<ItemUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemUI.SetPointerOverUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemUI.SetPointerOverUI(false);
    }
}
