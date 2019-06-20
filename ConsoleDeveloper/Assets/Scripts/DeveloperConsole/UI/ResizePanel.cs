using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Vector2 minSize;
    public Vector2 maxSize;

    private RectTransform rectTransform;
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;

    private void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null)
            return;

        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out currentPointerPosition);
        Vector2 resizableValue = currentPointerPosition - previousPointerPosition;

        sizeDelta += new Vector2(resizableValue.x, resizableValue.y);
        sizeDelta = new Vector2(Mathf.Clamp(sizeDelta.x,  minSize.x, maxSize.x),
                                Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y));

        rectTransform.sizeDelta = sizeDelta;

        previousPointerPosition = currentPointerPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out previousPointerPosition);
    }


    
}
