using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public int index;
    public bool isInHotBar;
    [SerializeField] private Image _avatar;
    private Image Avatar
    {
        get
        {
            if (_avatar == null)
            {
                _avatar = GetComponent<Image>();
            }
            return _avatar;
        }
    }
    private ItemDataScriptableObject _data;

    public void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (!_data) 
        {
            SetAvatar(null);
            return; 
        }
        _data.indexSlot = index;
        _avatar.sprite = Resources.Load<Sprite>("Items/" + _data.data.avatarName);
        SetAvatar(_avatar.sprite);
    }

    public void SetAvatar(Sprite avatar)
    {
        _avatar.sprite = avatar;
        if (avatar == null)
        {
            HideSlot();
        }
        else
        {
            DisplaySlot();
        }
    }
    private void HideSlot()
    {
        Color color = _avatar.color;
        color.a = 0;
        _avatar.color = color;
    }
    private void DisplaySlot()
    {
        Color color = _avatar.color;
        color.a = 255;
        _avatar.color = color;
    }
    public Sprite GetAvatar()
    {
        if (_avatar == null) return null ;
        return Avatar.sprite;
    }
    public void SetData(ItemDataScriptableObject data)
    {
        this._data = data;
        LoadData();
    }
    public ItemDataScriptableObject GetData()
    {
        return _data;
    }
}
