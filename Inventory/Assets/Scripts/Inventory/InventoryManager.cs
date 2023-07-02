using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int hotbarSlotsCount;
    [SerializeField] private int inventorySlotsCount;
    private ItemDataBase itemsInventory;
    [SerializeField] private GameObject slotItem;
    [SerializeField] private GameObject inventoryContainer;
    [SerializeField] private GameObject hotBarContainer;
    private List<GameObject> slotList = new List<GameObject>();
    int count = 0;
    [SerializeField] private GameObject alwaysOnTop;

    private const string SlotNamePrefix = "Item ";

    private void Awake()
    {
        itemsInventory = GetComponent<ItemDataBase>();
        DragAndDrop.OnMoveItem += MoveItem;
        DragAndDrop.OnSwapItems += SwapSlots;
    }
    private void Start()
    {
        int count = 0;
        for (int i = 0; i < inventorySlotsCount; i++)
        {
            GameObject slotItemGO = CreateSlotItem(inventoryContainer, count);
            slotList.Add(slotItemGO);
            count++;
        }

        for (int i = 0; i < hotbarSlotsCount; i++)
        {
            GameObject slotItemGO = CreateSlotItem(hotBarContainer, count, true);
            slotList.Add(slotItemGO);
            count++;
        }
        if (itemsInventory.Data != null)
        {
            int countIndex = 0;
            foreach (var item in itemsInventory.Data)
            {
                slotList[item.indexSlot].GetComponent<SlotItem>().SetData(item);
                slotList[item.indexSlot].GetComponent<SlotItem>().GetData().indexList = countIndex;
                countIndex++;
            }        
        }
    }
    private GameObject CreateSlotItem(GameObject parentContainer, int index, bool isInHotBar = false)
    {
        GameObject slotItemGO = Instantiate(slotItem, parentContainer.transform.localPosition, Quaternion.identity);
        slotItemGO.transform.SetParent(parentContainer.transform);
        slotItemGO.GetComponent<RectTransform>().localScale = Vector3.one;
        slotItemGO.name = SlotNamePrefix + index;

        var dragAndDrop = slotItemGO.GetComponent<SlotItem>().GetComponentInChildren<DragAndDrop>();
        dragAndDrop.AlwaysOnTop = alwaysOnTop;
        dragAndDrop.inventoryManager = this;

        var slotItemComponent = slotItemGO.GetComponent<SlotItem>();
        slotItemComponent.index = index;
        slotItemComponent.isInHotBar = isInHotBar;

        return slotItemGO;
    }
    public void MoveItem(SlotItem dragItem, SlotItem enterItem) // OK
    {

        var dragData = dragItem.GetData();
        itemsInventory.Data[dragData.indexList].indexSlot = enterItem.index;
        enterItem.SetData(dragData);
        dragItem.SetData(null);
    }

    public void SwapSlots(SlotItem firstItem, SlotItem secondItem)
    {
        var firstData = firstItem.GetData();
        var secondData = secondItem.GetData();

        int firstIndexList = firstData.indexList;
        int firstIndexSlot = firstData.indexSlot;
        int secondIndexList = secondData.indexList;
        int secondIndexSlot = secondData.indexSlot;

        itemsInventory.Data[firstIndexList].indexSlot = secondIndexSlot;
        itemsInventory.Data[secondIndexList].indexSlot = firstIndexSlot;

        firstItem.SetData(secondData);
        secondItem.SetData(firstData);
    }

    private void OnDisable()
    {
        var dragAndDrop = GetComponent<DragAndDrop>();
        DragAndDrop.OnMoveItem -= MoveItem;
        DragAndDrop.OnSwapItems -= SwapSlots;
    }

}
