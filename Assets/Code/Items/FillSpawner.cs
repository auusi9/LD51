using Code.Basic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Items
{
    public class FillSpawner: MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private GridCanvas _gridCanvas;
        [SerializeField] private AudioSource _spawnerAudioSource;
        [SerializeField] private Vector3 _positionCenter = new Vector3(130f, -40f, 0f);
        [SerializeField] private float _range = 100f;
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _pressClip;
        [SerializeField] private float _hoverAudioPitch = 0.5f;
        [SerializeField] private float _pressAudioPitch = 1.0f;
        private float _initialVolume;

        private void Start()
        {
            _initialVolume = _spawnerAudioSource.volume;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayAudioClip(_pressClip, 1f, _pressAudioPitch);

            Item fill = Instantiate(_itemPrefab, Vector3.zero, _itemPrefab.transform.rotation, _gridCanvas.Canvas.transform);

            Vector3 variation = new Vector3(Random.Range(-_range, _range), Random.Range(-_range, _range), 0f);
            Vector3 newPos = _positionCenter + variation;
            fill.transform.localPosition = newPos;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayAudioClip(_hoverClip, 0.3f, _hoverAudioPitch);
        }

        private void PlayAudioClip(AudioClip audioClip, float volume, float pitch)
        {
            _spawnerAudioSource.clip = audioClip;
            _spawnerAudioSource.volume = _initialVolume * volume;
            _spawnerAudioSource.pitch = pitch;
            _spawnerAudioSource.Play();
        }
    }
}