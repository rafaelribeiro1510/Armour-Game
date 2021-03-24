using System;
using Body;
using Body.BodyType;
using DG.Tweening;
using UnityEngine;

namespace Target
{
    public class TargetBehaviour : MonoBehaviour
    {
        [Header("Blinking parameters")]
        [SerializeField] private float blinkDuration;
        [SerializeField] private Color blinkColor;
        private Sequence _blinkTween;
    
        public BodyPartType BodyType;
        private TargetSprites _spriteController;
        private GameController _drawerController;
        private SpriteRenderer _renderer;
        private Color _originalColor;

        [SerializeField] private ReturnParameters returnParameters;
    
        BodyPartBehaviour _partHoveringOver = null;

        private void Awake() 
        {
            _renderer = GetComponent<SpriteRenderer>();
            _spriteController = GetComponent<TargetSprites>();
            _originalColor = _renderer.color;
        }

        private void Start()
        {
            _drawerController = GameController.Instance;
            _spriteController.SetSprite(BodyType);
        }

        private void Update()
        {
            if (_partHoveringOver && !_partHoveringOver.isGrabbed && !_partHoveringOver.finished) {
                _partHoveringOver.finished = true;

                Transform _t = _partHoveringOver.transform;
                DOTween.Kill(_t);
            
                _t.DOMove(transform.position, returnParameters.returnDuration/2)
                    .SetEase(returnParameters.returnEase)
                    .OnComplete(() => { _partHoveringOver.StartGlowing();}); // Ease to target
                _t.parent = null;
            
                StopGlowing();

                _drawerController.ActivatePair(null, null);
            }
        }

        private void StartBlink() 
        {
            _blinkTween = DOTween.Sequence();
            _blinkTween.Append(_renderer.DOColor(blinkColor, blinkDuration)).Append(_renderer.DOColor(_originalColor, blinkDuration)).SetLoops(Int32.MaxValue);
        }
    
        public void StartGlowing(){
            StartBlink();
        }

        public void StopGlowing(){
            _blinkTween.Kill(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(tag)) return;
            
            if (_partHoveringOver == null) _partHoveringOver = other.GetComponent<BodyPartBehaviour>();
            
            _partHoveringOver.onTopOfTarget = true;
            StartGlowing();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(tag)) return;
            
            _partHoveringOver.onTopOfTarget = false;
            if (_partHoveringOver)
            {
                StopGlowing();
                _partHoveringOver = null;
            }
        }
    }
}
