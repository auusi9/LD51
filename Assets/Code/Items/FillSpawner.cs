using Code.Basic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Items
{
    public class FillSpawner: MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private GridCanvas _gridCanvas;
        [SerializeField] private AudioSource _spawnerAudioSource;
        [SerializeField] private Vector3 _positionCenter = new Vector3(130f, -40f, 0f);
        [SerializeField] private float _range = 100f;

        public void OnPointerDown(PointerEventData eventData)
        {
            _spawnerAudioSource.Play();

            Item fill = Instantiate(_itemPrefab, Vector3.zero, _itemPrefab.transform.rotation, _gridCanvas.Canvas.transform);

            Vector3 variation = new Vector3(Random.Range(-_range, _range), Random.Range(-_range, _range), 0f);
            Vector3 newPos = _positionCenter + variation;
            fill.transform.localPosition = newPos;

            //Instantiate(_itemPrefab, Vector3.zero, _itemPrefab.transform.rotation, _gridCanvas.Canvas.transform);
        }
    }
}