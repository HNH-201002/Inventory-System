using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int hotbarSlotsCount;
    [SerializeField] private int inventorySlotsCount;
    [SerializeField] private GameObject slotItem;
    [SerializeField] private GameObject inventoryContainer;
    [SerializeField] private GameObject hotBarContainer;
    [SerializeField] private GameObject alwaysOnTop;
    [SerializeField] private GameManager gameManager;

    private List<GameObject> slotList = new List<GameObject>();

    private const string SlotNamePrefix = "Item ";
    public List<int> emptySlot = new List<int>();

    public event Action OnAddedItem;

    private void Awake()
    {
        DragAndDrop.OnMoveItem += MoveItem;
        DragAndDrop.OnSwapItems += SwapSlots;
        GameManager.OnFullStack += Add;
        GameManager.OnStackCompleteItem += StackCompleteItem;
        GameManager.OnStackWithRemainder += StackWithRemainder;
    }
    private void Start()
    {
        var data = gameManager.LoadData().Data;
        for (int i = 0; i < inventorySlotsCount; i++)
        {
            GameObject slotItemGO = CreateSlotItem(inventoryContainer,i);
            slotList.Add(slotItemGO);
            emptySlot.Add(1);
        }

        for (int i = 0; i < hotbarSlotsCount; i++)
        {
            GameObject slotItemGO = CreateSlotItem(hotBarContainer, i + inventorySlotsCount);
            slotList.Add(slotItemGO);
            emptySlot.Add(1);
        }
        if (data != null && data.Count > 0)
        {
            foreach (var key in data.Keys.ToList()) 
            {
                int count = 0;
                var itemList = data[key].ToList();

                foreach (var item in itemList)
                {
                    if (item.amount < 0)
                    {
                        data[key].Remove(item);
                        continue;
                    }
                    slotList[item.indexSlot].GetComponent<SlotItem>().SetData(item);
                    slotList[item.indexSlot].GetComponent<SlotItem>().GetData().indexList = count;
                    emptySlot[item.indexSlot] = 0;
                    count++;
                    if (item.amount < 64)
                    {
                        //TODO : ?
                    }
                }
            }
        }
    }
    private GameObject CreateSlotItem(GameObject parentContainer, int index)
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

        return slotItemGO;
    }
    public void MoveItem(SlotItem dragItem, SlotItem enterItem) 
    {
        var dragData = dragItem.GetData();

        enterItem.SetData(dragData);

        dragItem.SetData(null);

        emptySlot[dragItem.index] = 1;
        emptySlot[enterItem.index] = 0;
    }

    public void SwapSlots(SlotItem firstItem, SlotItem secondItem)
    {
        var firstData = firstItem.GetData();
        var secondData = secondItem.GetData();

        if (firstData.data.id == secondData.data.id)
        {
            gameManager.StackTwoItemHandler(firstData,secondData);
            return;
        }
        firstItem.SetData(secondData);
        secondItem.SetData(firstData);
    }
    public void Add(Items data,bool check = false)
    {
        if (data.GetData().data.canStack)
        {

        }
        else
        {
            for (int i = 0; i < emptySlot.Count; i++)
            {
                if (emptySlot[i] == 0) continue;

                slotList[i].GetComponent<SlotItem>().SetData(data.GetData());
                emptySlot[i] = 0;
                gameManager.AddItem(data);
                OnAddedItem?.Invoke();
                return;
            }
            print("Full inventory");
        }
    }
    public void StackCompleteItem(ItemsDTO dragItem,ItemsDTO enterItem) 
    {
        slotList[dragItem.indexSlot].GetComponent<SlotItem>().SetData(dragItem,false);
        emptySlot[dragItem.indexSlot] = 1;
        slotList[enterItem.indexSlot].GetComponent<SlotItem>().SetData(enterItem,true);
    }
    public void StackWithRemainder(ItemsDTO dragItem, ItemsDTO enterItem)
    {
        slotList[dragItem.indexSlot].GetComponent<SlotItem>().SetData(dragItem, true);
        slotList[enterItem.indexSlot].GetComponent<SlotItem>().SetData(enterItem, true);
    }
    private void OnDisable()
    {
        var dragAndDrop = GetComponent<DragAndDrop>();
        DragAndDrop.OnMoveItem -= MoveItem;
        DragAndDrop.OnSwapItems -= SwapSlots;

        GameManager.OnFullStack -= Add;
        GameManager.OnStackCompleteItem -= StackCompleteItem;
        GameManager.OnStackWithRemainder -= StackWithRemainder;
    }

}
