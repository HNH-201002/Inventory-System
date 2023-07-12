using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item", order = 0, fileName = "New item")]
[Serializable]
public class ItemScriptableObject : ScriptableObject
{
    public int id;
    public string Name;
    public string avatarName;
    public TypeItem typeItem;
    public bool canConsume;
    public bool canStack;
    [Serializable]
    public enum TypeItem
    {
        weapon,
        food
    }
}

