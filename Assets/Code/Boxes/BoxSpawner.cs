using Code.Basic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Boxes
{
    public class BoxSpawner : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private MovingBox _boxPrefab;
        [SerializeField] private GridCanvas _gridCanvas;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Instantiate(_boxPrefab, Vector3.zero, _boxPrefab.transform.rotation, _gridCanvas.Canvas.transform);
        }
    }
}