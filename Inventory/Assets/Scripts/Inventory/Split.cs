using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Split : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private bool isDragging = false;
    private PointerEventData _eventData;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null)
            return;
        if (!eventData.pointerEnter.GetComponent<SlotItem>())
            return;
        if (!eventData.pointerEnter.GetComponentInChildren<DragAndDrop>())
            return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isDragging = !isDragging;
            _eventData = eventData;
        }
    }
    public void FixedUpdate()
    {
        if (isDragging)
        {
            rectTransform.anchoredPosition += _eventData.delta / canvas.scaleFactor;
        }
    }
}
