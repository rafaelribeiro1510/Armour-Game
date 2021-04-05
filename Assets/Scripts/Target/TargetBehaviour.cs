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
    }
}
