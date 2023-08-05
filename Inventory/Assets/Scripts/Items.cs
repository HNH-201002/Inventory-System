using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Items : MonoBehaviour
{
    ItemsDTO data;
    [SerializeField] private ItemScriptableObject dataItems;
    public void Awake()
    {
        data = new ItemsDTO();
        data.data = dataItems;
        if (data.amount == 0)
        {
            data.amount = 1;
        }
    }
    public Items(ItemsDTO data)
    {
        this.data = data;
    }
    public int GetAmount()
    {
        return  data.amount;
    }
    public ItemsDTO GetData()
    {
        return data;
    }
}
[Serializable]
public class ItemsDTO
{
    public ItemsDTO()
    {
        // Khởi tạo các giá trị mặc định tại đây nếu cần thiết
        data = null;
        amount = 1;
        indexSlot = 0;
        indexList = 0;
    }
    public ItemsDTO(ItemsDTO itemsDTO)
    {
        data = itemsDTO.data;
        amount = itemsDTO.amount;
        indexList = itemsDTO.indexList;
        indexSlot = itemsDTO.indexSlot;
    }
    public ItemScriptableObject data;
    public int amount;
    public int indexSlot;
    public int indexList;
}

