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
    private ItemScriptableObject _data;

    public void Start()
    {
        if (!_data) { return; }
        _avatar.sprite = Resources.Load<Sprite>("Items/" + _data.avatarName);
        if (_avatar.sprite == null)
        {
            HideSlot();
        }
        else
        {
            DisplaySlot();
        }
    }

    public void SetAvatar(Sprite avatar)
    {
        _avatar.sprite = avatar;
        if (avatar == null )
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
    public void SetData(ItemScriptableObject data)
    {
        this._data = data;
    }
    public ItemScriptableObject GetData()
    {
        return _data;
    }
}
