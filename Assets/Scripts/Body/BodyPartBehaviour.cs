using System;
using Body.BodyType;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Body
{
    public class BodyPartBehaviour : MonoBehaviour
    {
        [Header("While Being Grabbed parameters")]
        [SerializeField] private float shrinkPercent;
        [SerializeField] private float growUpThreshold;

        [Header("While In Place parameters")] 
        [SerializeField] private float floatPower;
        [SerializeField] private float floatSpeed;
        [SerializeField] private Color blinkColor;
        private Sequence _glowTween;
    
        [SerializeField] private ReturnParameters returnParameters;

        private BodyPartSprites _spriteController;
        public BodyPartType BodyType;
        public BodyPartState BodyPartState;
        private float _startingScale;
        private Color _startingColor;
        [SerializeField] private float scaleInsideDrawer;
    
        [HideInInspector] public bool isGrabbed;
        [HideInInspector] public bool onTopOfTarget = false;
        [HideInInspector] public bool finished = false;
        [HideInInspector] public bool insideDrawer = true;

        private Camera _camera;
        [SerializeField] private Collider2D _collider;
        private Transform _parentTransform;
        private SpriteRenderer _renderer;

        void Awake() {
            _camera = Camera.main;
            _parentTransform = transform.parent;
            _startingScale = transform.localScale.x;
            _renderer = GetComponent<SpriteRenderer>();
            _startingColor = _renderer.color;
        
        
            _spriteController = GetComponentInChildren<BodyPartSprites>();
        }

        private void Start()
        {
            _spriteController.SetSprite(BodyType, BodyPartState);
            _collider = _spriteController.UpdateCollider();
        
            scaleInsideDrawer = 
                BodyType == BodyPartType.Head ? 
                    0.8f : 
                    (BodyType == BodyPartType.LegL || BodyType == BodyPartType.LegR ? 0.3f : 0.35f);
            transform.DOScale(scaleInsideDrawer, 0.00001f);
        }

        void Update()
        {
            if (insideDrawer && Vector3.Distance(transform.position, _parentTransform.position) > growUpThreshold)
            {
                insideDrawer = false;
                transform.DOScale(_startingScale, 0.1f);
            }
            else if (Vector3.Distance(transform.position, _parentTransform.position) <= growUpThreshold)
            {
                insideDrawer = true;
                transform.DOScale(scaleInsideDrawer, 0.1f);
            }
            
            if (finished) return; 
            
            if (Input.touchCount > 0){
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                    {
                        if (_collider == Physics2D.OverlapPoint(touchPos)){
                            isGrabbed = true;
                            transform.DOScale(shrinkPercent * _startingScale, 0.1f);
                        }

                        break;
                    }
                
                    case TouchPhase.Moved when isGrabbed:
                        transform.position = touchPos;
                        break;
                
                    case TouchPhase.Ended when isGrabbed:
                    {
                        isGrabbed = false;
                        transform.DOScale(_startingScale, 0.1f);

                        if (transform.position != _parentTransform.position){
                            if (!onTopOfTarget && !finished) {
                                ReturnToDrawer();
                            }
                        }

                        break;
                    }
                }
            }
        }

        public void ReturnToDrawer()
        {
            StopGlowing();
            isGrabbed = true;
            transform.DOMove(_parentTransform.position, returnParameters.returnDuration)
                .SetEase(returnParameters.returnEase)
                .OnComplete(() => {
                    transform.SetParent(_parentTransform);
                    isGrabbed = false;
                });
            
            finished = false;
        }

        public void EaseIntoPlace(Vector3 targetPosition)
        {
            DOTween.Kill(transform);
            transform.DOMove(targetPosition, returnParameters.returnDuration / 2)
                .SetEase(returnParameters.returnEase)
                .OnComplete(() => { transform.parent = null; });
            
            //_drawerController.ActivatePair(null, null);
        }

        public void SetState(BodyPartType bodyPartType, BodyPartState bodyPartState)
        {
            BodyType = bodyPartType;
            BodyPartState = bodyPartState;
            _spriteController.SetSprite(bodyPartType, bodyPartState);
        }

        public void StartGlowing()
        {
            _glowTween = DOTween.Sequence();
            _glowTween.Append(_renderer.DOColor(blinkColor, floatSpeed)).Append(_renderer.DOColor(_startingColor, floatSpeed)).SetLoops(Int32.MaxValue);
        }
        
        public void StopGlowing(){
            _glowTween.Kill(true);
        }
    }
}
