using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableObjectController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform OwnerRect;
    private CanvasGroup OwnerCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        OwnerRect = this.gameObject.GetComponent<RectTransform>();
        OwnerCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OwnerCanvasGroup.alpha = 0.6f;
        OwnerCanvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OwnerCanvasGroup.alpha = 1.0f;
        OwnerCanvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OwnerRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
