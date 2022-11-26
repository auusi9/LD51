using Code.Basic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Boxes
{
    public class BoxSpawner : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private MovingBox _boxPrefab;
        [SerializeField] private GridCanvas _gridCanvas;
        [SerializeField] private AudioSource _spawnerAudioSource;
        private Vector3 _positionCenter = new Vector3(130f, -40f, 0f);
        private float range = 100f;

        public void OnPointerDown(PointerEventData eventData)
        {
            _spawnerAudioSource.Play();
            MovingBox box = Instantiate(_boxPrefab, Vector3.zero, _boxPrefab.transform.rotation, _gridCanvas.Canvas.transform) as MovingBox;

            Vector3 variation = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0f);
            Vector3 newPos = _positionCenter + variation;
            box.transform.localPosition = newPos;
        }
    }
}