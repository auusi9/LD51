using Code.Basic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Boxes
{
    public class BoxSpawner : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MovingBox _boxPrefab;
        [SerializeField] private GridCanvas _gridCanvas;

        private DraggableObject _lastBox;
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_lastBox != null)
            {
                _lastBox.OnDrag(eventData);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_lastBox != null)
            {
                _lastBox.OnBeginDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_lastBox != null)
            {
                _lastBox.OnEndDrag(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector3 position = _gridCanvas.Canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition);
            var canvasTransform = _gridCanvas.Canvas.transform;
            position.z = canvasTransform.position.z;
            
            _lastBox = Instantiate(_boxPrefab, position, _boxPrefab.transform.rotation, canvasTransform);
            _lastBox.OnPointerDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_lastBox != null)
            {
                _lastBox.OnPointerUp(eventData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_lastBox != null)
            {
                _lastBox.OnPointerEnter(eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_lastBox != null)
            {
                _lastBox.OnPointerExit(eventData);
            }
        }
    }
}