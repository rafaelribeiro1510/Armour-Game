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
    Sequence floatTween;
    
    [SerializeField] ReturnParameters returnParameters;

    BodyPartSprites spriteController;
    public BodyPartType bodyBodyPartType;
    public BodyPartState bodyBodyPartState;
    float startingScale;
    
    [HideInInspector] public bool _isGrabbed;
    [HideInInspector] public bool _onTopOfTarget = false;
    [HideInInspector] public bool _finished = false;
    
    Camera _camera;
    [SerializeField] Collider2D _collider;
    Transform _parentTransform;

    void Awake() {
        _camera = Camera.main;
        _parentTransform = transform.parent;
        startingScale = transform.localScale.x;
        
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
                    transform.DOScale(shrinkPercent * startingScale, 0.1f);
                }
            }

            else if (touch.phase == TouchPhase.Moved && _isGrabbed){
                transform.position = touchPos;
            }

            else if (touch.phase == TouchPhase.Ended){
                _isGrabbed = false;
                transform.DOScale(startingScale, 0.1f);

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

    public void StartFloating()
    {
        floatTween = DOTween.Sequence();
        floatTween
            .Append(transform.DOBlendableMoveBy(new Vector3(0,floatPower,0), floatSpeed))
            .Append(transform.DOBlendableMoveBy(new Vector3(0,-floatPower,0), floatSpeed))
            .SetLoops(Int32.MaxValue).SetEase(Ease.OutCubic);
    }
}
