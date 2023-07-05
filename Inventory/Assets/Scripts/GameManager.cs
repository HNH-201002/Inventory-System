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
    public ItemDataBase LoadData()
    {   
        return ItemsData;
    }

    public void AddItem(Items data)
    {
        if (ItemsData.Data.TryGetValue(data.GetData().data.id, out List<ItemsDTO> list))
        {
            Items newItem = new Items(data.GetData());
            list.Add(newItem.GetData());
        }
        else
        {
            List<ItemsDTO> newList = new List<ItemsDTO>();
            Items newItem = new Items(data.GetData());
            newList.Add(newItem.GetData());
            ItemsData.Data.Add(data.GetData().data.id, newList);
        }
    }
}
