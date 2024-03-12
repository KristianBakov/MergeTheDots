using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - _startPosition;
        Debug.Log("Drag delta: " + delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _endPosition = eventData.position;

        Vector2 swipeVector = _endPosition - _startPosition;

        if (swipeVector.magnitude > 50) // Minimum swipe distance threshold
        {
            if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                if (swipeVector.x > 0)
                    Debug.Log("Swiped right");
                else
                    Debug.Log("Swiped left");
            }
            else
            {
                if (swipeVector.y > 0)
                    Debug.Log("Swiped up");
                else
                    Debug.Log("Swiped down");
            }
        }
    }
}
