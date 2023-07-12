using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private GameObject spawnContainer;
    [SerializeField] private List<TestItem> items;
    float posZ = 0;
    public void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        foreach (var item in items)
        {
            float posX = 0f;
            posZ += 1;
            for (int i = 0; i < item.amount; i++)
            {
                posX += 0.5f;
                GameObject itemGO = Instantiate(item.item, spawnPos + new Vector3(posX, 0, posZ), Quaternion.identity);
                itemGO.transform.parent = spawnContainer.transform;
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            posZ = 0;
            Spawn();
        }
    }
}
[Serializable]
struct TestItem
{
    public int amount;
    public GameObject item;
}
