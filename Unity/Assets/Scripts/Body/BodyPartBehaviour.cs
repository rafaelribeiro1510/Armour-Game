﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BodyPartBehaviour : MonoBehaviour
{
    private Camera _camera;

    public BodyPartType bodyBodyPartType;
    public BodyPartState bodyBodyPartState;
    BodyPartSprites spriteController;

    [Header("Return parameters")]
    [SerializeField] float returnDuration;
    [SerializeField] Ease returnEase = Ease.Linear;
    
    [Header("While Being Grabbed parameters")]
    [SerializeField] float shrinkPercent;

    bool _isGrabbed;
    bool _notOnTopOfTarget = true;
    
    Collider2D _collider;
    Vector3 _initialPosition;

    TargetBehaviour _currHovering;

    void Awake() {
        _camera = Camera.main;
        _collider = GetComponent<Collider2D>();    
        _initialPosition = transform.position;

        spriteController = GetComponentInChildren<BodyPartSprites>();
        spriteController.SetSprite(bodyBodyPartType, bodyBodyPartState);
    }

    void Update()
    {
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began){
                if (_collider == Physics2D.OverlapPoint(touchPos)){
                    _isGrabbed = true;
                    transform.DOScale(shrinkPercent, 0.1f);
                }
            }

            else if (touch.phase == TouchPhase.Moved && _isGrabbed){
                transform.position = touchPos;
            }

            else if (touch.phase == TouchPhase.Ended){
                _isGrabbed = false;
                transform.DOScale(1, 0.1f);

                if (transform.position != _initialPosition){
                    if (_notOnTopOfTarget)  
                        transform.DOMove(_initialPosition, returnDuration).SetEase(returnEase); // Ease to starting position
                    else{
                        transform.DOMove(_currHovering.transform.position, returnDuration/2).SetEase(returnEase); // Ease to target
                        _currHovering.stopGlowing();
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Target")){
            if (_currHovering == null) _currHovering = other.GetComponent<TargetBehaviour>();

            if (_currHovering.BodyType == this.bodyBodyPartType){
                _notOnTopOfTarget = false;
                _currHovering.startGlowing();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Target")){
            _notOnTopOfTarget = true;
            _currHovering.stopGlowing();
            _currHovering = null;
        }
    }

    public void SetState(BodyPartType bodyPartType, BodyPartState bodyPartState)
    {
        bodyBodyPartType = bodyPartType;
        bodyBodyPartState = bodyPartState;
        spriteController.SetSprite(bodyPartType, bodyPartState);
    }
}