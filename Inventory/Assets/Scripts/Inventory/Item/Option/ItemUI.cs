using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject uiMenuPrefab; // Prefab cho UI menu
    [SerializeField] private GameObject canvas;
    private GameObject uiMenuInstance;
    private bool isPointerOverUI;
    private void Awake()
    {
        Inventory_UI_Manager.OnMenuOpenClose += SetActiveOption;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotItem>()) return;
        if (eventData.button == PointerEventData.InputButton.Right && eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotItem>().hasData)
        {
            Vector2 canvasPosition = eventData.position;    
            canvasPosition = canvas.transform.InverseTransformPoint(canvasPosition);
            if (uiMenuInstance != null)
            {
                uiMenuInstance.SetActive(true);
                uiMenuInstance.transform.localPosition = new Vector2(canvasPosition.x + 70, canvasPosition.y - 15);
            }
            else
            {
                uiMenuInstance = Instantiate(uiMenuPrefab, new Vector2(canvasPosition.x + 70, canvasPosition.y - 15), Quaternion.identity);
                uiMenuInstance.GetComponent<PointerUIManager>().itemUI = this;
                uiMenuInstance.transform.SetParent(canvas.transform, false);
            }
            uiMenuInstance.GetComponent<OptionItem>().SetSlotItem(eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotItem>());
        }
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && uiMenuInstance != null && !isPointerOverUI)
        {
            SetActiveOption(false);
        }
    }

    private void SetActiveOption(bool active)
    {
        if (uiMenuInstance == null) return;
        uiMenuInstance.SetActive(false);
    }

    public void SetPointerOverUI(bool isOverUI)
    {
        isPointerOverUI = isOverUI;
    }
}