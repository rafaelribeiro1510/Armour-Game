using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BodyPartBehaviour : MonoBehaviour
{
    [Header("While Being Grabbed parameters")]
    [SerializeField] float shrinkPercent;

    [Header("While In Place parameters")] 
    [SerializeField] float floatPower;
    [SerializeField] float floatSpeed;
    [SerializeField] Color blinkColor;
    Sequence glowTween;
    
    [SerializeField] ReturnParameters returnParameters;

    BodyPartSprites spriteController;
    public BodyPartType bodyBodyPartType;
    public BodyPartState bodyBodyPartState;
    float _startingScale;
    Color _startingColor;
    
    [HideInInspector] public bool _isGrabbed;
    [HideInInspector] public bool _onTopOfTarget = false;
    [HideInInspector] public bool _finished = false;
    
    Camera _camera;
    [SerializeField] Collider2D _collider;
    Transform _parentTransform;
    SpriteRenderer _renderer;

    void Awake() {
        _camera = Camera.main;
        _parentTransform = transform.parent;
        _startingScale = transform.localScale.x;
        _renderer = GetComponent<SpriteRenderer>();
        _startingColor = _renderer.color;
        
        spriteController = GetComponentInChildren<BodyPartSprites>();
    }

    private void Start()
    {
        spriteController.SetSprite(bodyBodyPartType, bodyBodyPartState);
        _collider = GetComponentInChildren<Collider2D>();
    }

    void Update()
    {
        if (_finished) return; 
            
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began){
                if (_collider == Physics2D.OverlapPoint(touchPos)){
                    _isGrabbed = true;
                    transform.DOScale(shrinkPercent * _startingScale, 0.1f);
                }
            }

            else if (touch.phase == TouchPhase.Moved && _isGrabbed){
                transform.position = touchPos;
            }

            else if (touch.phase == TouchPhase.Ended){
                _isGrabbed = false;
                transform.DOScale(_startingScale, 0.1f);

                if (transform.position != _parentTransform.position){
                    if (!_onTopOfTarget)  
                        transform.DOMove(_parentTransform.position, returnParameters.returnDuration).SetEase(returnParameters.returnEase); // Ease to starting position
                }
            }
        }
    }

    public void SetState(BodyPartType bodyPartType, BodyPartState bodyPartState)
    {
        bodyBodyPartType = bodyPartType;
        bodyBodyPartState = bodyPartState;
        spriteController.SetSprite(bodyPartType, bodyPartState);
    }

    public void StartGlowing()
    {
        glowTween = DOTween.Sequence();
        glowTween.Append(_renderer.DOColor(blinkColor, floatSpeed)).Append(_renderer.DOColor(_startingColor, floatSpeed)).SetLoops(Int32.MaxValue);
    }
}
