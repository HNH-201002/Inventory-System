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
    [SerializeField] private int amountOfSlot;
    private ItemDataBase itemsInventory;
    [SerializeField] private GameObject slotItem;
    [SerializeField] private GameObject inventoryContainer;
    [SerializeField] private GameObject hotBarContainer;
    private List<GameObject> slotHotBar = new List<GameObject>();
    private List<GameObject> slotInventory = new List<GameObject>();
    private List<GameObject> itemInHotBar;
    int count = 0;
    [SerializeField] private GameObject alwaysOnTop;
    bool isSpawn;

    private void Awake()
    {
        itemsInventory = GetComponent<ItemDataBase>();
        if (isSpawn) return;
        foreach (var slot in hotBarContainer.GetComponentsInChildren<SlotItem>())
        {
            slotHotBar.Add(slot.gameObject);
            slot.index = count;
            slot.isInHotBar = true;
            count++;
        }
    }
    private void Start()
    {
        int count = 0;
        for (int i = 0; i < amountOfSlot; i++)
        {
            GameObject slotItemGO = Instantiate(slotItem, inventoryContainer.transform.localPosition, Quaternion.identity);

            slotItemGO.transform.SetParent(inventoryContainer.transform);
            slotItemGO.GetComponent<RectTransform>().localScale = Vector3.one;
            slotItemGO.name = "Item " + count;

            slotItemGO.GetComponent<SlotItem>().GetComponentInChildren<DragAndDrop>().AlwaysOnTop = alwaysOnTop;
            slotItemGO.GetComponent<SlotItem>().GetComponentInChildren<DragAndDrop>().inventoryManager = this;
            slotItemGO.GetComponent<SlotItem>().index = count;
            slotInventory.Add(slotItemGO);
            count++;
        }
        if (itemsInventory.Data != null)
        {
            int countIndex = 0;
            foreach (var item in itemsInventory.Data)
            {
                if (item.isInHotBar)
                {
                    slotHotBar[item.indexSlot].GetComponent<SlotItem>().SetData(item);
                    continue;
                }
                else
                {
                    slotInventory[item.indexSlot].GetComponent<SlotItem>().SetData(item);
                    slotInventory[item.indexSlot].GetComponent<SlotItem>().GetData().indexList = countIndex;
                    countIndex++;
                }
            }        
        }
        isSpawn = true;
    }

    public void ChangeSlotItem(SlotItem item,int newIndex)
    {
        slotInventory[newIndex].GetComponent<SlotItem>().SetData(itemsInventory.Data[item.GetData().indexList]);
        itemsInventory.Data[item.GetData().indexList].indexSlot = newIndex;
        item.SetData(null);
    }
    public void SwapSlotItem(SlotItem firstItem, SlotItem secondItem)
    {
        var firstData = itemsInventory.Data[firstItem.GetData().indexList];
        var secondData = itemsInventory.Data[secondItem.GetData().indexList];
        int tempIndexSlot = itemsInventory.Data[firstItem.GetData().indexList].indexSlot;
        itemsInventory.Data[firstItem.GetData().indexList].indexSlot = itemsInventory.Data[secondItem.GetData().indexList].indexSlot;
        itemsInventory.Data[secondItem.GetData().indexList].indexSlot = tempIndexSlot;

        firstItem.SetData(secondData);
        secondItem.SetData(firstData);
    }

    public void AddItemToInventory(SlotItem item)
    {

    }

    public void RemoveItemFromInventory(SlotItem item)
    {

    }

    public void MoveItemToHotbar(SlotItem item, int hotbarIndex)
    {

    }

    public void RemoveItemFromHotbar(SlotItem item)
    {

    }
    private void OnApplicationQuit()
    {

    }
}
