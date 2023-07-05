using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ItemDataBase ItemsData;

    public static event Action<Items> OnAmountOfItemChanged;
    public static event Action<Items,bool> OnFullStack;
    public static event Action<ItemsDTO,ItemsDTO> OnStackCompleteItem;
    public static event Action<ItemsDTO, ItemsDTO> OnStackWithRemainder;
    public ItemDataBase LoadData()
    {   
        return ItemsData;
    }

    public void AddItem(Items data)
    {
        if (ItemsData.Data.TryGetValue(data.GetData().data.id, out List<ItemsDTO> list))
        {
            Items newItem = new Items(data.GetData());
            newItem.GetData().amount = 1;
            newItem.GetData().indexList = list.Count;
            list.Add(newItem.GetData());
        }
        else
        {
            List<ItemsDTO> newList = new List<ItemsDTO>();
            Items newItem = new Items(data.GetData());
            newItem.GetData().amount = 1;
            newItem.GetData().indexList = list.Count;
            newList.Add(newItem.GetData());
            ItemsData.Data.Add(data.GetData().data.id, newList);
        }
    }
    public void StackHander(HashSet<int> nonFullStackItems,ItemsDTO data)
    {
        if (nonFullStackItems.Contains(data.data.id))
        {
            //TODO: ?
        }
    }
    public void StackTwoItemHandler(ItemsDTO dragItem,ItemsDTO enterItem)
    {
        if (ItemsData.Data.TryGetValue(enterItem.data.id, out List<ItemsDTO> list))
        {
            int quantity = dragItem.amount + enterItem.amount;
            if (quantity < 64)
            {
                list[enterItem.indexList].amount += dragItem.amount;
                list[dragItem.indexList].amount = -1;
                dragItem.data = null;
                OnStackCompleteItem?.Invoke(dragItem, list[enterItem.indexList]);
            }
            else
            {
                int temp = list[enterItem.indexList].amount + dragItem.amount - 64;
                list[enterItem.indexList].amount = 64;
                list[dragItem.indexList].amount = temp;
                OnStackWithRemainder?.Invoke(list[dragItem.indexList], list[enterItem.indexList]);
            }
        }
        else
        {
            Debug.LogError("Cannot figure out this item");
            return;
        }
    }
}
