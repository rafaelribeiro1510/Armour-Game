using System.Collections.Generic;
using System.Linq;
using Body.BodyType;
using DG.Tweening;
using UnityEngine;

namespace PartCompleteMenu
{
    public class PartCompleteScript : MonoBehaviour
    {
        public static PartCompleteScript Instance { get; private set; }
        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }
    
        [SerializeField] private float openingDuration;
        [HideInInspector] public bool open;
    
        private Transform _transform;
        private Vector3 _closedPosition;

        private List<SizeSelector> _sizeSelectors;
        private List<EmotionInput> _emotionInputs;

        public BodyInputInfo Result;

        private void Awake()
        {
            SingletonInitialization();
            _transform = GetComponent<Transform>();
            _closedPosition = _transform.position;

            _sizeSelectors = new List<SizeSelector>(GetComponentsInChildren<SizeSelector>());
            _emotionInputs = new List<EmotionInput>(GetComponentsInChildren<EmotionInput>());
        }

        private void Update()
        {
            if (!open) return;
        
            // If some selector isn't ready yet
            if (_sizeSelectors.Any(sizeSelector => !sizeSelector.ready) /*|| _emotionInputs.Any(emotionInput => !emotionInput.ready)*/) { return; }
        
            Result = new BodyInputInfo(
                _sizeSelectors[0].output, _sizeSelectors[1].output,
                _emotionInputs[0].output, _emotionInputs[1].output);
        }

        [ContextMenu("Open Size and Emotion Menu")]
        public void Open()
        {
            if (open) return;
            open = true;
            _transform.DOMove(Vector3.zero, openingDuration).SetEase(Ease.OutSine);
        }

        [ContextMenu("Close Size and Emotion Menu")]
        public void Close()
        {
            if (!open) return;
            open = false;
            _transform.DOMove(_closedPosition, openingDuration).SetEase(Ease.OutSine);
        }

        public void ResetValues()
        {
            Result = null;
            _sizeSelectors.ForEach(x => x.ResetValues());
            _emotionInputs.ForEach(x => x.ResetValues());
            Close();
        }
    }
}
