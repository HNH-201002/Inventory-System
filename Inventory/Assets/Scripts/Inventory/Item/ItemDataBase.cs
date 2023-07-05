using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDataBase : MonoBehaviour 
{
    private DataManager instance;

    string path;
    public Dictionary<int,List<ItemsDTO>> Data { get; set; }
    [SerializeField] private  List<Items> dataAddInEdit;
    public void Awake()
    {
        Data = new Dictionary<int, List<ItemsDTO>>();
        instance = DataManager.Instance;
        path = Application.persistentDataPath + "/data.json";
        if (instance.Load(path) != null)
        {
            Data = instance.Load(path);
        }
        if (dataAddInEdit != null)
        {
            foreach (Items item in dataAddInEdit)
            {
                if (Data.TryGetValue(item.GetData().data.id,out List<ItemsDTO> list))
                {
                    Items newItem = new Items(item.GetData());
                    list.Add(newItem.GetData());
                }
                else
                {
                    List<ItemsDTO> newList = new List<ItemsDTO>();
                    Items newItem = new Items(item.GetData());
                    newList.Add(newItem.GetData());
                    Data.Add(newItem.GetData().data.id,newList);
                }
            }
        }
    }
    public void Save()
    {
        instance.Save(Data, path);
    }
    private void OnApplicationQuit()
    {
        if (instance != null && Data != null)
        {
            instance.Save(Data, path);
            print("Saved");
        }
    }
}



