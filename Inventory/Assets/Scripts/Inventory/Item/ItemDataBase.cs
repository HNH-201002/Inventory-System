using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour 
{
    private DataManager instance;

    string path;
    public List<ItemDataScriptableObject> Data { get; set; }
    [SerializeField] private List<ItemDataScriptableObject> dataAddInEdit;
    public void Awake()
    {
        Data = new List<ItemDataScriptableObject>();
        instance = DataManager.Instance;
        path = Application.persistentDataPath + "/data.json";
        if (instance.Load(path) != null)
        {
            Data = instance.Load(path);
        }
        if (dataAddInEdit != null)
        {
            foreach (ItemDataScriptableObject item in dataAddInEdit)
            {
                Data.Add(item);
            }
        }
    }

    private void OnApplicationQuit()
    {
        Data.Sort((item1, item2) => item1.index.CompareTo(item2.index));
        instance.Save(Data, path);
        print("Saved");
    }
}



