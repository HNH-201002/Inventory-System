using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData", order = 1, fileName = "New item Data")]
[Serializable]
public class ItemDataScriptableObject : ScriptableObject
{
    public ItemScriptableObject data;
    public int amount;
    public int indexSlot;
    public int indexList;
}
