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
        var itemData = data.GetData();
        var itemId = itemData.data.id;

        if (ItemsData.Data.TryGetValue(itemId, out var list))
        {
            var newItem = new Items(itemData);
            newItem.GetData().amount = 1;
            newItem.GetData().indexList = list.Count;
            list.Add(newItem.GetData());
        }
        else
        {
            var newList = new List<ItemsDTO>();
            var newItem = new Items(itemData);
            newItem.GetData().amount = 1;
            newItem.GetData().indexList = 0;
            newList.Add(newItem.GetData());
            ItemsData.Data.Add(itemId, newList);
        }
    }

    public void StackTwoItemHandler(ItemsDTO dragItem,ItemsDTO enterItem)
    {
        if (!ItemsData.Data.TryGetValue(enterItem.data.id, out var list))
        {
            Debug.LogError("Cannot find this item");
            return;
        }

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
            int remainder = quantity - 64;
            list[enterItem.indexList].amount = 64;
            list[dragItem.indexList].amount = remainder;
            OnStackWithRemainder?.Invoke(list[dragItem.indexList], list[enterItem.indexList]);
        }

    }
}
