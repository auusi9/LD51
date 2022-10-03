using Code.Basic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Items
{
    public class FillSpawner: MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private GridCanvas _gridCanvas;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Instantiate(_itemPrefab, Vector3.zero, _itemPrefab.transform.rotation, _gridCanvas.Canvas.transform);
        }
    }
}