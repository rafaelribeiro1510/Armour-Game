using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SingleDrawerBehaviour : MonoBehaviour
{
    [HideInInspector] public SingleDrawerBehaviour pair;
    DrawersController _controller;

    private Camera _camera;
    [HideInInspector] public Transform _transform;
    private Vector3 _transformPosition;
    
    [Header("Drawer stuff")]
    [SerializeField] float moveRange;
    bool _placedHorizontally;
    bool _placedOnTheLeft;
    bool _placedOnTheBottom;
    [SerializeField] private float dragScale;
    
    [HideInInspector] public float _movementPercentage = 0;
    Vector3 _startingPosition;
    Vector3 _finalPosition;
    Vector3 _targetPosition;

    [Header("While Being Grabbed parameters")]
    [SerializeField] float shrinkPercent = 1.05f;
    Vector2 _grabPos;
    Collider2D _collider;
    bool _isGrabbed;


    private void Awake() {
        _camera = Camera.main;
        _collider = GetComponent<Collider2D>();
        _transform = transform;
        
        _placedHorizontally = _transform.rotation.z == 0;
        _placedOnTheLeft = _transform.position.x < 0;
        _placedOnTheBottom = _transform.position.y < 0;

        _startingPosition = _transform.position;
        _finalPosition = _startingPosition + 
        new Vector3(
                    (_placedHorizontally?moveRange:0)*(_placedOnTheLeft?1:-1), 
                    (_placedHorizontally?0:moveRange)*(_placedOnTheBottom?1:-1)
                    );
    }

    private void Start()
    {
        _controller = DrawersController.Instance;
    }

    void Update()
    {
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began){
                if (_collider == Physics2D.OverlapPoint(touchPos)){
                    _isGrabbed = true;
                    _transform.DOScale(shrinkPercent, 0.1f);
                    pair._transform.DOScale(shrinkPercent, 0.1f);
                    _grabPos = touchPos;
                }
            }

            else if (touch.phase == TouchPhase.Moved && _isGrabbed){
                Vector2 difference = touchPos - _grabPos; difference.Scale(new Vector2(dragScale,dragScale));
                _targetPosition = _placedHorizontally 
                ? new Vector3(_transform.position.x + difference.x, _transform.position.y) 
                : new Vector3(_transform.position.x, _transform.position.y + difference.y);

                _targetPosition = Utils.NormalizedWithBounds(_targetPosition, _startingPosition, _finalPosition);
                _movementPercentage = Utils.Vector3InverseLerp(_startingPosition, _finalPosition, _targetPosition);
                
                // Make sure pairs move together 
                if (pair) pair._movementPercentage = _movementPercentage;
            }

            else if (touch.phase == TouchPhase.Ended && _isGrabbed){
                _isGrabbed = false;
                _transform.DOScale(1, 0.1f);
                pair._transform.DOScale(1, 0.1f);
                
                // So drawers don't stay stuck in awkward mid positions
                if (_movementPercentage < 0.75f) {
                    Close();
                }
                else if (_movementPercentage >= 0.75f) {
                    Open();
                    _controller.ActivatePair(this, pair);
                }
            }
        }
        
        _transform.position = Vector3.Lerp(_startingPosition, _finalPosition, _movementPercentage);
    }

    private void Open()
    {
        DOTween.To(() => _movementPercentage, x => _movementPercentage = x, 1, 0.1f);
        DOTween.To(() => pair._movementPercentage, x =>pair. _movementPercentage = x, 1, 0.1f);
    }

    public void Close() {
        DOTween.To(() => _movementPercentage, x => _movementPercentage = x, 0, 0.25f);
        DOTween.To(() => pair._movementPercentage, x => pair._movementPercentage = x, 0, 0.25f);
    }
}
