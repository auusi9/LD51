using System;
using System.Collections.Generic;
using Code.Burners;
using Code.ConveyorBelts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Basic
{
    public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private float _scale = 1.0f;
        [SerializeField] protected GridCanvas _gridCanvas;
        [SerializeField] protected GridCanvas _burner;
        [SerializeField] protected GridCanvas _sender;

        public event Action RightClickEvent;

        private Vector3 _moveOffset;
        private Vector3 _oldScale;

        public virtual bool HasPool => false;
        
        protected virtual void ReturnToPool()
        {
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_gridCanvas.Canvas.transform as RectTransform, eventData.position, _gridCanvas.Canvas.worldCamera, out Vector2 localPoint);
            _moveOffset = transform.position - _gridCanvas.Canvas.transform.TransformPoint(localPoint);
            OnDrag(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_gridCanvas.Canvas.transform as RectTransform, eventData.position, _gridCanvas.Canvas.worldCamera, out Vector2 localPoint);
            transform.position = _gridCanvas.Canvas.transform.TransformPoint(localPoint) + _moveOffset;
            
            Vector3[] fourCornersArray = new Vector3[4];
            _gridCanvas.Parent.GetWorldCorners(fourCornersArray);

            Vector3 pos = _rectTransform.position;

            pos.x = Mathf.Clamp(pos.x, fourCornersArray[0].x, fourCornersArray[2].x);
            pos.y = Mathf.Clamp(pos.y, fourCornersArray[0].y, fourCornersArray[2].y);
            _rectTransform.position = pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _shadow.enabled = false;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClickEvent?.Invoke();
                return;
            }
            _oldScale = transform.localScale;
            transform.localScale *= _scale;
            _shadow.enabled = true;
            transform.SetAsLastSibling();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            transform.localScale *= 1 / _scale;
            _shadow.enabled = false;
            TrySetIntoBurner(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }

        public virtual void SetBelt(Belt belt)
        {
            
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
        
        private void TrySetIntoBurner(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            _burner.GraphicRaycaster.Raycast(eventData, results);
            
            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<Burner>();
                if (slot != null)
                {
                    Destroy();
                    break;
                }
            }
        }
    }
}