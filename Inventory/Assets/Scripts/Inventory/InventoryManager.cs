using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int hotbarSlotsCount;
    [SerializeField] private int inventorySlotsCount;

    [SerializeField] private GameObject slotItem;
    [SerializeField] private GameObject grabSlotItem;

    [SerializeField] private GameObject inventoryContainer;
    [SerializeField] private GameObject hotBarContainer;
    [SerializeField] private GameObject alwaysOnTop;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject canvas;

 

    private List<GameObject> slotList = new List<GameObject>();

    private const string SlotNamePrefix = "Item ";
    private List<int> emptySlot = new List<int>();
    private Dictionary<int,List<ItemsDTO>> listCanStack = new Dictionary<int, List<ItemsDTO>>();

    public event Action OnAddedItem;


    private GameObject grabSlotItemGO;
    private Vector3 mousePositionOffset;
    private bool isGrabbing = false;

    private void Awake()
    {
        DragAndDrop.OnMoveItem += MoveItem;
        DragAndDrop.OnSwapItems += SwapSlots;
        GameManager.OnFullStack += Add;
        GameManager.OnStackCompleteItem += StackCompleteItem;
        GameManager.OnStackWithRemainder += StackWithRemainder;
        OptionItem.OnSubmitAmountOfItem += SplitItem;
        Split.OnSplitItem += HandleItemAfterSplit;
    }
    private void Start()
    {
        var data = gameManager.LoadData().Data;

        var inventorySlotItems = Enumerable.Range(0, inventorySlotsCount)
                                 .Select(i => CreateSlotItem(inventoryContainer,i))
                                 .ToList();
        slotList.AddRange(inventorySlotItems);
        emptySlot.AddRange(Enumerable.Repeat(1,inventorySlotsCount));

        var hotbarSlotItems = Enumerable.Range(0,hotbarSlotsCount)
                              .Select(i => CreateSlotItem(hotBarContainer,i + inventorySlotsCount))
                              .ToList();
        slotList.AddRange(hotbarSlotItems);
        emptySlot.AddRange(Enumerable.Repeat(1,hotbarSlotsCount));


        if (data != null && data.Count > 0)
        {
            foreach (var key in data.Keys.ToList()) 
            {
                int count = 0;
                var itemList = data[key].ToList();
                data[key].RemoveAll(item => item.data == null || item.amount < 0);
                itemList.RemoveAll(item => item.data == null || item.amount < 0);
                foreach (var item in itemList)
                {
                    if (item.data == null)
                    {
                        data.Remove(item.indexList);
                        continue;
                    }
                    var slotItem = slotList[item.indexSlot].GetComponent<SlotItem>();
                    slotItem.SetData(item);
                    slotItem.GetData().indexList = count;
                    emptySlot[item.indexSlot] = 0;
                    count++;

                    if (!(item.amount < 64 && item.data.canStack)) 
                        continue;

                    if (listCanStack.TryGetValue(item.data.id, out var list))
                    {
                        list.Add(item);
                    }
                    else
                    {
                        listCanStack.Add(item.data.id,new List<ItemsDTO> { item });
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

        var slotItemGOComponent = slotItemGO.GetComponent<SlotItem>();
        var dragAndDrop = slotItemGOComponent.GetComponentInChildren<DragAndDrop>();
        dragAndDrop.AlwaysOnTop = alwaysOnTop;
        dragAndDrop.inventoryManager = this;
        slotItemGOComponent.index = index;
        return slotItemGO;
    }
    public void MoveItem(SlotItem dragItem, SlotItem enterItem) 
    {
        var dragData = dragItem.GetData();

        enterItem.SetData(dragData);

        dragItem.SetData(null,false);

        emptySlot[dragItem.index] = 1;
        emptySlot[enterItem.index] = 0;
    }

    public void SwapSlots(SlotItem firstItem, SlotItem secondItem)
    {
        var firstData = firstItem.GetData();
        var secondData = secondItem.GetData();

        if (firstData.data.id == secondData.data.id && firstData.data.canStack)
        {
            gameManager.StackTwoItemHandler(firstData,secondData);
            return;
        }
        firstItem.SetData(secondData);
        secondItem.SetData(firstData);
    }
    public void Add(Items data,bool check = false)
    {
        var itemData = data.GetData().data;
        if (itemData.canStack)
        {
            if (listCanStack.TryGetValue(itemData.id,out List<ItemsDTO> list))
            {
                foreach (var item in list.ToList())
                {
                    if (item.amount >= 64)
                    {
                        list.Remove(item);
                        continue;
                    }
                    item.amount++;
                    slotList[item.indexSlot].GetComponent<SlotItem>().SetData(item);
                    OnAddedItem?.Invoke();
                    return;
                }
                list.Add(data.GetData());
            }
            else 
            { 
                listCanStack.Add(itemData.id,new List<ItemsDTO> { data.GetData()});
            }
        }

        FindSlotInInventory(data);
    }

    private void FindSlotInInventory(Items data)
    {
        int emptySlotIndex = emptySlot.FindIndex(slot => slot == 1);

        if (emptySlotIndex != -1)
        {
            slotList[emptySlotIndex].GetComponent<SlotItem>().SetData(data.GetData());
            emptySlot[emptySlotIndex] = 0;

            gameManager.AddItem(data);
            OnAddedItem?.Invoke();
            return;
        }
        Debug.LogWarning("Full inventory");
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
    public void SplitItem(int slotItem, int amount)
    {
        SlotItem slotItemComponent = slotList[slotItem].GetComponent<SlotItem>();

        slotItemComponent.GetData().amount -= amount;
        ItemsDTO itemsDTO = slotItemComponent.GetData();
        slotItemComponent.SetData(itemsDTO);

        // Tạo một bản sao mới để truyền dữ liệu đến GameObject grabSlotItemGO
        ItemsDTO grabItemsDTO = new ItemsDTO(slotItemComponent.GetData());
        grabItemsDTO.amount = amount;

        GameObject grabSlotItemGO = Instantiate(grabSlotItem);
        grabSlotItemGO.GetComponent<Split>().SetData(grabItemsDTO);
        grabSlotItemGO.GetComponentInChildren<DragAndDrop>().AlwaysOnTop = alwaysOnTop;
        grabSlotItemGO.GetComponentInChildren<DragAndDrop>().inventoryManager = this;

        grabSlotItemGO.transform.SetParent(alwaysOnTop.transform);
    }

    public void HandleItemAfterSplit(ItemsDTO data, SlotItem slotItemGO, GameObject goj)
    {
        SlotItem slotItemComponent = slotItemGO.GetComponent<SlotItem>();

        if (slotItemComponent.hasData)
        {
            ItemsDTO slotData = slotItemComponent.GetData();
            if (slotData.indexSlot == data.indexSlot)
            {
                gameManager.StackOriginalItem(data, slotData);
            }
            else if (slotData.data.avatarName == data.data.avatarName)
            {
                gameManager.StackTwoItemHandler(data, slotData);
            }
            else
            {
                Debug.LogWarning("Not same type item");
            }
        }
        else
        {
            slotList[slotItemGO.index].GetComponent<SlotItem>().SetData(data);
            emptySlot[slotItemGO.index] = 0;

            Items items = new Items(data);
            gameManager.AddItem(items);
        }

        goj.SetActive(false);
    }
    private void OnDisable()
    {
        var dragAndDrop = GetComponent<DragAndDrop>();
        DragAndDrop.OnMoveItem -= MoveItem;
        DragAndDrop.OnSwapItems -= SwapSlots;

        GameManager.OnFullStack -= Add;
        GameManager.OnStackCompleteItem -= StackCompleteItem;
        GameManager.OnStackWithRemainder -= StackWithRemainder;

        Split.OnSplitItem -= HandleItemAfterSplit;
    }

}
