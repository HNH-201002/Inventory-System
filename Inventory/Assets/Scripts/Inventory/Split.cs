using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Split : MonoBehaviour
{
    [SerializeField] private Image _avatar;
    [SerializeField] private TMP_Text _amount;
    public static Action<ItemsDTO,SlotItem,GameObject> OnSplitItem; // Sự kiện trả về cả item và vị trí slot
    ItemsDTO data;

    public void SetData(ItemsDTO data)
    {
        this.data = data;
        _avatar.sprite = Resources.Load<Sprite>("Items/" + data.data.avatarName);
        _amount.text = data.amount.ToString();
    }

    private void Update()
    {
        gameObject.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastToUI();
        }
    }
    private void RaycastToUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject != gameObject)
            {
                if (result.gameObject.GetComponentInParent<SlotItem>())
                {
                    OnSplitItem?.Invoke(data,result.gameObject.GetComponentInParent<SlotItem>(),gameObject);
                }
                break; 
            }
        }
    }
}
