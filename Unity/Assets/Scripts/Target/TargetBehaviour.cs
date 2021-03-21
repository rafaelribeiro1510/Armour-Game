using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class TargetBehaviour : MonoBehaviour
{
    [Header("Blinking parameters")]
    [SerializeField] float blinkDuration;
    [SerializeField] Color blinkColor;
    Sequence blinkTween;
    
    public BodyPartType BodyType;
    TargetSprites _spriteController;
    DrawersController _drawerController;
    SpriteRenderer _renderer;
    Color _originalColor;

    [SerializeField] ReturnParameters returnParameters;
    
    BodyPartBehaviour _partHoveringOver = null;

    private void Awake() 
    {
        _renderer = GetComponent<SpriteRenderer>();
        _spriteController = GetComponent<TargetSprites>();
        _originalColor = _renderer.color;
    }

    private void Start()
    {
        _drawerController = DrawersController.Instance;
        _spriteController.SetSprite(BodyType);
    }

    private void Update()
    {
        if (_partHoveringOver && !_partHoveringOver._isGrabbed && !_partHoveringOver._finished) {
            _partHoveringOver._finished = true;
            
            Transform _t = _partHoveringOver.transform;
            _t.DOMove(transform.position, returnParameters.returnDuration/2)
                .SetEase(returnParameters.returnEase)
                .OnComplete(() => { _partHoveringOver.StartFloating();}); // Ease to target
            _t.parent = null;
            
            StopGlowing();

            //_drawerController.ActivatePair(null, null);
        }
    }

    void StartBlink() 
    {
        blinkTween = DOTween.Sequence();
        blinkTween.Append(_renderer.DOColor(blinkColor, blinkDuration)).Append(_renderer.DOColor(_originalColor, blinkDuration)).SetLoops(Int32.MaxValue);
    }
    
    public void StartGlowing(){
        StartBlink();
    }

    public void StopGlowing(){
        blinkTween.Kill(true);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(tag)){
            if (_partHoveringOver == null) _partHoveringOver = other.GetComponent<BodyPartBehaviour>();
            
            _partHoveringOver._onTopOfTarget = true;
            StartGlowing();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag(tag)){
            _partHoveringOver._onTopOfTarget = false;
            if (_partHoveringOver)
            {
                StopGlowing();
                _partHoveringOver = null;
            }
        }
    }
}
