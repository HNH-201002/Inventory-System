using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("DataManager");
                    instance = obj.AddComponent<DataManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save(Dictionary<int, List<ItemsDTO>> data, string path)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        if (!File.Exists(path))
        {
            using (File.Create(path)) { }
        }

        File.WriteAllText(path, jsonData);
    }
    public Dictionary<int,List<ItemsDTO>> Load(string path)
    {
        if (File.Exists(path))
        {
            // Đọc nội dung của tệp JSON
            string jsonData = File.ReadAllText(path);

            // Chuyển đổi JSON thành danh sách đối tượng
            Dictionary<int, List<ItemsDTO>> data = JsonConvert.DeserializeObject<Dictionary<int, List<ItemsDTO>>>(jsonData);
            print("load successfully");
            return data;
        }
        else
        {
            Debug.LogWarning("No saved data found.");
            return new Dictionary<int, List<ItemsDTO>>();
        }
    }
}
