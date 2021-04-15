using Body;
using DG.Tweening;
using UnityEngine;

namespace Drawer
{
    public class DrawerBehaviour : MonoBehaviour
    {
        private BodyPartBehaviour _holdingPart;
        private Transform _holdingPartTransform;
        [HideInInspector] public DrawerBehaviour pair;
        private DrawerController _drawerController;
        private BodyController _bodyController;
        private Transform _whiteBarTransform;

        private Camera _camera;
        [HideInInspector] public Transform _transform;
        private Vector3 _transformPosition;
    
        [Header("Drawer stuff")]
        [SerializeField] private float moveRange;

        [SerializeField] private float autoCloseDistance;

        private bool _active = true;
        private bool _placedHorizontally;
        private bool _placedOnTheLeft;
        private bool _placedOnTheBottom;
        [SerializeField] private float dragScale;
        [SerializeField] private float doubleTapCooldownTime;
        [SerializeField] private float autoClosingCooldownTime;
        [SerializeField] private float slideOffScreenAmount;
        private float _doubleTapCooldown;
        private float _autoClosingCooldown;
    
        [HideInInspector] public float movementPercentage;
        Vector3 _startingPosition;
        Vector3 _finalPosition;
        Vector3 _targetPosition;

        [Header("While Being Grabbed parameters")]
        [SerializeField] private float shrinkPercent = 1.05f;

        private Vector2 _grabPos;
        private Collider2D _collider;
        private bool _isGrabbed;
        
        private void Awake() {
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();
            _transform = transform;
            _whiteBarTransform = _transform.GetChild(1);
        
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
            _drawerController = DrawerController.Instance;
            _bodyController = BodyController.Instance;
        }

        public void setHoldingPart(BodyPartBehaviour bodyPartBehaviour)
        {
            _holdingPart = bodyPartBehaviour;
            _holdingPartTransform = _holdingPart.transform;
        }

        private void Update()
        {
            if (!_active) return;
            TickCooldowns();

            if (_autoClosingCooldown <= 0 && !_holdingPart.inPlace && Vector3.Distance(_transform.position, _holdingPartTransform.position) > autoCloseDistance)
            {
                _autoClosingCooldown = autoClosingCooldownTime;
                Close();
                return;
            }
        
            if (Input.touchCount > 0){
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                if (_doubleTapCooldown <= 0 && touch.tapCount == 2)
                {
                    _doubleTapCooldown = doubleTapCooldownTime;
                    Close();
                    return;
                }
            
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                    {
                        if (_collider == Physics2D.OverlapPoint(touchPos)){
                            _isGrabbed = true;
                            _transform.DOScale(shrinkPercent, 0.1f);
                            pair._transform.DOScale(shrinkPercent, 0.1f);
                            _grabPos = touchPos;
                        }

                        break;
                    }
                    
                    case TouchPhase.Moved when _isGrabbed:
                    {
                        Vector2 difference = touchPos - _grabPos; difference.Scale(new Vector2(dragScale,dragScale));
                        _targetPosition = _placedHorizontally 
                            ? new Vector3(_transform.position.x + difference.x, _transform.position.y) 
                            : new Vector3(_transform.position.x, _transform.position.y + difference.y);

                        _targetPosition = Utils.Utils.NormalizedWithBounds(_targetPosition, _startingPosition, _finalPosition);
                        movementPercentage = Utils.Utils.Vector3InverseLerp(_startingPosition, _finalPosition, _targetPosition);

                        if (movementPercentage >= 0.5f)
                        {
                            _drawerController.ActivatePair(this, pair);
                            _bodyController.TryOpeningDrawer(_holdingPart.BodyType, pair._holdingPart.BodyType);
                        }
                        
                        // Make sure pairs move together 
                        if (pair) pair.movementPercentage = movementPercentage;
                        break;
                    }
                    
                    case TouchPhase.Ended when _isGrabbed:
                    {
                        _isGrabbed = false;
                        _transform.DOScale(1, 0.1f);
                        pair._transform.DOScale(1, 0.1f);
                
                        // So drawers don't stay stuck in awkward mid positions
                        if (movementPercentage < 0.75f) {
                            Close();
                        }
                        else if (movementPercentage >= 0.75f) {
                            Open();
                        }

                        break;
                    }
                }
            }
        
            _transform.position = Vector3.Lerp(_startingPosition, _finalPosition, movementPercentage);
        }

        private void TickCooldowns()
        {
            if (_doubleTapCooldown > 0) _doubleTapCooldown -= Time.deltaTime;
            if (_autoClosingCooldown > 0) _autoClosingCooldown -= Time.deltaTime;
            
        }

        private void Open()
        {
            DOTween.To(() => movementPercentage, x => movementPercentage = x, 1, 0.1f);
            DOTween.To(() => pair.movementPercentage, x =>pair. movementPercentage = x, 1, 0.1f);
        }

        public void Close()
        {
            if (_isGrabbed || pair._isGrabbed) return;
            DOTween.To(() => movementPercentage, x => movementPercentage = x, 0, 0.25f);
            DOTween.To(() => pair.movementPercentage, x => pair.movementPercentage = x, 0, 0.25f);
        }

        public void SlideOffScreen()
        {
            var amountX = (_placedHorizontally ? 1 : 0) * (_placedOnTheLeft ? -1 : 1) * slideOffScreenAmount;
            var amountY = (_placedHorizontally ? 0 : 1) * (_placedOnTheBottom ? -1 : 1) * slideOffScreenAmount;
            _active = false;
            _whiteBarTransform.DOBlendableMoveBy(new Vector3(amountX, amountY, _transform.position.z), 0.2f);
            _transform.DOBlendableMoveBy(new Vector3(amountX, amountY, _transform.position.z), 0.2f);
        }
        
        public void SlideOnScreen()
        {
            var amountX = (_placedHorizontally ? 1 : 0) * (_placedOnTheLeft ? -1 : 1) * -slideOffScreenAmount;
            var amountY = (_placedHorizontally ? 0 : 1) * (_placedOnTheBottom ? -1 : 1) * -slideOffScreenAmount;
            _whiteBarTransform.DOBlendableMoveBy(new Vector3(amountX, amountY, _transform.position.z), 0.2f);
            _transform.DOBlendableMoveBy(new Vector3(amountX, amountY, _transform.position.z), 0.2f).OnComplete(() => _active = true);
        }
    }
}
