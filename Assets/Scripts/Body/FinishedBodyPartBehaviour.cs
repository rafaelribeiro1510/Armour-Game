using System;
using Body.BodyType;
using DG.Tweening;
using Stats;
using Target;
using UnityEngine;
using Utils;

namespace Body
{
    public class FinishedBodyPartBehaviour : MonoBehaviour
    {
        private FinishedBodyPartSprites _spriteController;
        public BodyPartType BodyType;
        private BodyPartState _bodyPartState;
        private BodyInputInfo _bodyInputInfo;

        private StatsScreenController _statsController;
        
        private Camera _camera;
        private Collider2D _collider;
        private SpriteRenderer _renderer;

        void Awake() {
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<SpriteRenderer>();
            if (_bodyPartState == BodyPartState.Disease)
            {
                var c = _renderer.color;
                _renderer.color = new Color(c.r, c.g, c.b, 0.5f);
            }
            
            _spriteController = GetComponentInChildren<FinishedBodyPartSprites>();

            _bodyPartState = GetComponentInParent<FinishedBody>().bodyPartState;

            _statsController = StatsScreenController.Instance;
        }

        public void SetBodyInputInfo(BodyInputInfo bodyInputInfo)
        {
            _bodyInputInfo = bodyInputInfo;
            
            _spriteController.SetSprite(BodyType, _bodyPartState, _bodyPartState == BodyPartState.Physical ? bodyInputInfo.SizePhysical : bodyInputInfo.SizeDisease);
            _collider = _spriteController.UpdateCollider();
        }

        void Update()
        {
            if (Input.touchCount > 0){
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Ended:
                    {
                        if (_collider == Physics2D.OverlapPoint(touchPos))
                        {
                            _statsController.SetBodyType(BodyType);
                        }
                        
                        break;   
                    }
                }
            }
        }
    }
}
