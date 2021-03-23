using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BodyPartBehaviour : MonoBehaviour
{
    [Header("While Being Grabbed parameters")]
    [SerializeField] float shrinkPercent;
    [SerializeField] float growUpThreshold;

    [Header("While In Place parameters")] 
    [SerializeField] float floatPower;
    [SerializeField] float floatSpeed;
    [SerializeField] Color blinkColor;
    Sequence glowTween;
    
    [SerializeField] ReturnParameters returnParameters;

    BodyPartSprites spriteController;
    public BodyPartType BodyPartType;
    public BodyPartState BodyPartState;
    float _startingScale;
    Color _startingColor;
    [SerializeField] float _scaleInsideDrawer;
    bool _insideDrawer = true;
    
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
        spriteController.SetSprite(BodyPartType, BodyPartState);
        _collider = spriteController.UpdateCollider();
        
        _scaleInsideDrawer = BodyPartType == BodyPartType.Head ? 0.5f : (BodyPartType == BodyPartType.LegL || BodyPartType == BodyPartType.LegR ? 0.3f : 0.35f);
        transform.DOScale(_scaleInsideDrawer, 0.00001f);
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
                    if (!_onTopOfTarget && !_finished)  
                        transform.DOMove(_parentTransform.position, returnParameters.returnDuration).SetEase(returnParameters.returnEase); // Ease to starting position
                }
            }
        }
        
        if (_insideDrawer && Vector3.Distance(transform.position, _parentTransform.position) > growUpThreshold)
        {
            _insideDrawer = false;
            transform.DOScale(_startingScale, 0.1f);
        }
        else if (Vector3.Distance(transform.position, _parentTransform.position) <= growUpThreshold)
        {
            _insideDrawer = true;
            transform.DOScale(_scaleInsideDrawer, 0.1f);
        }
    }

    public void SetState(BodyPartType bodyPartType, BodyPartState bodyPartState)
    {
        BodyPartType = bodyPartType;
        BodyPartState = bodyPartState;
        spriteController.SetSprite(bodyPartType, bodyPartState);
    }

    public void StartGlowing()
    {
        glowTween = DOTween.Sequence();
        glowTween.Append(_renderer.DOColor(blinkColor, floatSpeed)).Append(_renderer.DOColor(_startingColor, floatSpeed)).SetLoops(Int32.MaxValue);
    }
}
