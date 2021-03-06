﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Console
{
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {

        private Vector2 pointerOffset;
        private RectTransform canvasRectTransform;
        private RectTransform panelRectTransform;

        public void OnDrag(PointerEventData eventData)
        {
            if (panelRectTransform == null)
                return;

            Vector2 pointerPostion = ClampToWindow(eventData);

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, pointerPostion, eventData.pressEventCamera, out localPointerPosition))
            {
                panelRectTransform.localPosition = localPointerPosition - pointerOffset;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Becomes at top
            panelRectTransform.SetAsLastSibling();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset);
        }

        private void Awake()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if(canvas != null)
            {
                canvasRectTransform = canvas.transform as RectTransform;
                panelRectTransform = transform.parent as RectTransform;
            }
        }

        private Vector2 ClampToWindow(PointerEventData data)
        {
            Vector2 rawPointerPosition = data.position;

            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(canvasCorners);

            float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
            float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

            Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
            return newPointerPosition;
        }
    }
}
