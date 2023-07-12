using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private float distance;
    [SerializeField] private float radius;
    RaycastHit hit;
    int layerMask = 1 << 6;
    Ray ray;

    public void Start()
    {
        inventoryManager.OnAddedItem += HandlerGameObject;
        ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
    }

    void FixedUpdate()
    {
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;

        if (Physics.SphereCast(ray, radius, out hit, distance, layerMask))
        {
            if (hit.collider.GetComponent<Items>() && Input.GetKey(KeyCode.E))
            {
                HandlerGrabItem();
            }
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
        }
    }

    private void HandlerGrabItem()
    {
        inventoryManager.Add(hit.collider.GetComponent<Items>(), true);
    }

    private void HandlerGameObject()
    {
        Destroy(hit.collider.gameObject);
    }
}