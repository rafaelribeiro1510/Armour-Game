using System;
using System.Collections;
using Body;
using Body.BodyType;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Target
{
    public class TargetBehaviour : MonoBehaviour
    {
        [SerializeField] private TargetController _controller;
        
        [Header("Blinking parameters")]
        [SerializeField] private float blinkDuration;
        [SerializeField] private Color blinkColor;
        private Sequence _blinkTween;
    
        public BodyPartType BodyType;
        private TargetSprites _spriteController;
        private SpriteRenderer _renderer;
        private Collider2D _col;
        private Color _originalColor;

        [SerializeField] private ReturnParameters returnParameters;
    
        public BodyPartBehaviour partHoveringOver = null;

        private void Awake() 
        {
            _renderer = GetComponent<SpriteRenderer>();
            _spriteController = GetComponent<TargetSprites>();
            _col = GetComponent<Collider2D>();
            _originalColor = _renderer.color;
        }

        private void Start()
        {
            _spriteController.SetSprite(BodyType);
            _controller = TargetController.Instance;
        }

        private void Update()
        {
            if (partHoveringOver && !partHoveringOver.isGrabbed && !partHoveringOver.finished)
            {
                partHoveringOver.finished = true;

                if (_controller.TryPlacing(partHoveringOver))
                    partHoveringOver.EaseIntoPlace(transform.position);

                StopGlowing();
                
            }
        }

        private void TemporarilyDeactivateCollider()
        {
            StartCoroutine(TemporarilyDeactivateCollider_CR());
        }

        private IEnumerator TemporarilyDeactivateCollider_CR()
        {
            _col.enabled = false;
            yield return new WaitForSeconds(0.4f);
            _col.enabled = true;
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

            var tempPartHoveringOver = other.GetComponent<BodyPartBehaviour>();
            if (tempPartHoveringOver.insideDrawer) return;

            if (partHoveringOver is null) partHoveringOver = tempPartHoveringOver;
            
            partHoveringOver.onTopOfTarget = true;
            StartGlowing();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(tag)) return;
            
            if (!(partHoveringOver is null))
            {
                partHoveringOver.onTopOfTarget = false;
                StopGlowing();
                partHoveringOver = null;
            }
        }
    }
}
