using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour
{
    [SerializeField] private GameObject amountOfItemGO;
    private GameObject uiMenuInstance;

    private TMP_InputField amountOfItem;
    private Button submit;
    private SlotItem slotItem;

    public static Action<int,int> OnSubmitAmountOfItem;
    public void Awake()
    {
        Inventory_UI_Manager.OnMenuOpenClose += SetActiveGameObject;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && uiMenuInstance != null && uiMenuInstance.activeSelf)
        {
            uiMenuInstance.SetActive(false);
        }
    }
    public void Split()
    {
        if (uiMenuInstance != null)
        {
            uiMenuInstance.SetActive(true);
            return;
        }

        RectTransform canvasRectTransform = transform.parent.GetComponent<RectTransform>();

        Vector2 screenPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenPos, Camera.main, out Vector3 worldPos);

        uiMenuInstance = Instantiate(amountOfItemGO, worldPos, Quaternion.identity);
        uiMenuInstance.transform.SetParent(canvasRectTransform, false);
        amountOfItemGO.SetActive(true);

        amountOfItem = uiMenuInstance.GetComponentInChildren<TMP_InputField>();
        submit = uiMenuInstance.GetComponentInChildren<Button>();
        submit.onClick.AddListener(SubmitAmountOfItem);
    }
    private void SubmitAmountOfItem()
    {
        int amountOfItemText;
        if (slotItem == null)
        {
            Debug.LogError("Slot Item not found");
            return;
        }
        if (!int.TryParse(amountOfItem.text, out int amount) || amount > slotItem.GetData().amount || slotItem.GetData().amount == 1)
        {
            Debug.LogWarning("Amount of Item is not valid or greater than the available amount");
            return;
        }
        if (!slotItem.GetData().data.canStack) 
        {
            Debug.LogWarning("Item is not stack type");
            return;
        }

        amountOfItemText = amount;

        OnSubmitAmountOfItem?.Invoke(slotItem.GetData().indexSlot, amount);

        // Đặt lại trạng thái ban đầu của UI
        uiMenuInstance.SetActive(false);
        amountOfItem.text = "";
    }


    public void SetActiveGameObject(bool check)
    {
        if (uiMenuInstance == null) return;
        uiMenuInstance.SetActive(false);
        amountOfItem.text = "";
    }
    public void SetSlotItem(SlotItem slotItem)
    {
        this.slotItem = slotItem;
    }
}
