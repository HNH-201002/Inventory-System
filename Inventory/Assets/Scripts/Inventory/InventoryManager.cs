using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private ItemDataBase itemsInventory;
    [SerializeField] private GameObject slotItem;
    [SerializeField] private GameObject inventoryContainer;
    [SerializeField] private GameObject hotBarContainer;
    private List<GameObject> slotHotBar = new List<GameObject>();
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
        if (itemsInventory.Data != null)
        {
            foreach (var item in itemsInventory.Data)
            {
                if (item.isInHotBar)
                {
                    slotHotBar[item.index].GetComponent<SlotItem>().SetData(item.data);
                    continue;
                }
                GameObject slotItemGO = Instantiate(slotItem, inventoryContainer.transform.localPosition, Quaternion.identity);
                slotItemGO.transform.SetParent(inventoryContainer.transform);
                slotItemGO.GetComponent<RectTransform>().localScale = Vector3.one;
                slotItemGO.GetComponent<SlotItem>().GetComponentInChildren<DragAndDrop>().AlwaysOnTop = alwaysOnTop;
                slotItemGO.GetComponent<SlotItem>().SetData(item.data);
                slotItemGO.GetComponent<SlotItem>().GetComponentInChildren<DragAndDrop>().InventoryManager = this;
            }
        }
        isSpawn = true;
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
