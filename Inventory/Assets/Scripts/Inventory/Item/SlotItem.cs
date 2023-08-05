using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public int index;
    [SerializeField] private Image _avatar;
    [SerializeField] private TMP_Text _textMeshPro;

    public bool hasData = false;
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
    private ItemsDTO _data;

    public void Awake()
    {
        if (_textMeshPro == null) { _textMeshPro = GetComponent<TMP_Text>(); }
        if (_avatar == null) { _avatar = GetComponent<Image>(); }
    }
    public void Start()
    {
        LoadData();
    }

    private void LoadData()
    {

        if (!hasData) 
        {
            SetAvatar(null);
            _textMeshPro.text = "";
            return; 
        }

        _textMeshPro.transform.SetAsLastSibling();

        _textMeshPro.text = _data.amount.ToString();
        _data.indexSlot = index;
        if (_avatar != null && _data.data != null)
        {
            _avatar.sprite = Resources.Load<Sprite>("Items/" + _data.data.avatarName);
            SetAvatar(_avatar.sprite);
        }
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
        return _avatar?.sprite;
    }
    public void SetData(ItemsDTO data,bool check = true)
    {
        _data = data;
        hasData = check;
        LoadData();
    }
    public ItemsDTO GetData()
    {
        return _data;
    }
}
