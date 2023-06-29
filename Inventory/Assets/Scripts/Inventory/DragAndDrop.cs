using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour , IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public GameObject AlwaysOnTop;
    public InventoryManager InventoryManager;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector2 _oriPosition;
    private Image _image;
    Sprite currentAvatar;
    Transform parentDrag;
    string _tagAvatar = "Avatar";
    SlotItem itemGameObject;
    Color imageColor;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponent<Image>();
        _canvas = GetComponentInParent<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
     
        parentDrag = transform.parent;
        transform.SetParent(AlwaysOnTop.transform);
        transform.SetAsLastSibling();
        if (eventData.pointerDrag.tag != _tagAvatar) return;
        currentAvatar = _image.sprite;
        _oriPosition = _rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag.tag != _tagAvatar) return;
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
        if (eventData.pointerDrag == null || eventData.pointerEnter == null) return;

        SlotItem item = eventData.pointerEnter.GetComponentInParent<SlotItem>();
        Image image = eventData.pointerEnter.GetComponentInChildren<Image>();
        itemGameObject = gameObject.GetComponentInParent<SlotItem>();
        if (!item || !image) { return; }
        if (image.sprite == null)
        {
            itemGameObject.SetAvatar(null);
        }
        else
        {
            imageColor = gameObject.GetComponent<Image>().color;
            imageColor.a = 255;
            gameObject.GetComponent<Image>().color = imageColor;
            itemGameObject.SetAvatar(image.sprite);
        }
        item.SetAvatar(currentAvatar);

        if (item.isInHotBar)
        {
            InventoryManager.MoveItemToHotbar(itemGameObject,item.index);
        }
    }
}
