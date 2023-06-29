using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject InventoryPanel;
    public delegate void MenuOpenCloseEventHandler(bool isOpen);
    public static event MenuOpenCloseEventHandler OnMenuOpenClose;

    bool toggle = false;
    private void Start()
    {
        OnMenuOpenClose?.Invoke(false);
        InventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            toggle = !toggle;
            if (toggle)
            {
                InventoryPanel.SetActive(true);
                OnMenuOpenClose?.Invoke(true);
            }
            else
            {
                InventoryPanel.SetActive(false);
                OnMenuOpenClose?.Invoke(false);
            }
        }
    }
}
