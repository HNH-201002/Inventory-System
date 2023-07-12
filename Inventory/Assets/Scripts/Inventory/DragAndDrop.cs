using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour , IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public GameObject AlwaysOnTop;
    public InventoryManager inventoryManager;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector2 _oriPosition;
    Transform parentDrag;

    public delegate void ItemMoveDelegate(SlotItem dragItem, SlotItem enterItem);

    public static event ItemMoveDelegate OnMoveItem;
    public static event ItemMoveDelegate OnSwapItems;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentDrag = transform.parent;
        if (gameObject.GetComponent<DragAndDrop>() == null || eventData.button != PointerEventData.InputButton.Left) return;
        transform.SetParent(AlwaysOnTop.transform);
        transform.SetAsLastSibling();
        _oriPosition = _rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.pointerDrag) { return; }
        if (eventData.pointerDrag.GetComponent<DragAndDrop>().GetComponent<Image>().sprite == null) return;
        if (eventData.pointerDrag.GetComponent<DragAndDrop>() == null)
            return;
        if(eventData.button != PointerEventData.InputButton.Left) { return; }
        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = _oriPosition;
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentDrag.transform);
        eventData.pointerDrag.GetComponentInParent<SlotItem>().GetComponentInChildren<TMP_Text>().transform.SetAsLastSibling();
        if (!eventData.pointerDrag || !eventData.pointerEnter) { return; }

        if (gameObject.GetComponent<DragAndDrop>() == null
              ||
              eventData.pointerEnter.GetComponent<DragAndDrop>() == null ||
             gameObject.GetComponentInParent<SlotItem>().GetData() == null)
        {
            return;
        }
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        SlotItem enterItem = eventData.pointerEnter.GetComponentInParent<SlotItem>();
        Image enterItemImage = eventData.pointerEnter.GetComponent<Image>();
        SlotItem dragItem = gameObject.GetComponentInParent<SlotItem>();

        // Case 1: The slot drop does not have an item
        if (enterItemImage.sprite == null)
        {
            OnMoveItem?.Invoke(dragItem, enterItem);
        }
        // Case 2: The slot drop has an item
        else
        {
            OnSwapItems?.Invoke(dragItem, enterItem);
        }

    }
    public void ResetPosition()
    {
        _rectTransform.anchoredPosition = _oriPosition;
    }
}
