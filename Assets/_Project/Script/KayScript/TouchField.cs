using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 TouchDelta { get; private set; }
    private Vector2 _lastPosition;
    private int _pointerId = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_pointerId != -1) return; // Chỉ nhận 1 ngón tay cho vùng này
        _pointerId = eventData.pointerId;
        _lastPosition = eventData.position;
        TouchDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == _pointerId)
        {
            TouchDelta = eventData.position - _lastPosition;
            _lastPosition = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == _pointerId)
        {
            _pointerId = -1;
            TouchDelta = Vector2.zero;
        }
    }

    private void LateUpdate()
    {
        // Reset giá trị vuốt mỗi frame để tránh bị trôi camera
        TouchDelta = Vector2.zero;
    }
}
