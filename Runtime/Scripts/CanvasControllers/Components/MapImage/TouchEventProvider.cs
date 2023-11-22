using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchEventProvider : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public UnityEvent<PointerEventData> dragEvent;
    [SerializeField] public UnityEvent<PointerEventData> pointerDownEvent;
    [SerializeField] public UnityEvent<PointerEventData> pointerUpEvent;

    public void OnDrag(PointerEventData eventData)
    {
       dragEvent.Invoke(eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownEvent.Invoke(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pointerUpEvent.Invoke(eventData);
    }
}
