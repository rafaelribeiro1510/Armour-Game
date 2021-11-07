using System;
using Body.BodyType;
using DG.Tweening;
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

        private bool _isTouched = false; 
        
        private Camera _camera;
        private Collider2D _collider;
        private SpriteRenderer _renderer;

        void Awake() {
            _camera = Camera.main;
            _renderer = GetComponent<SpriteRenderer>();
            _spriteController = GetComponentInChildren<FinishedBodyPartSprites>();

            _bodyPartState = GetComponentInParent<FinishedBody>().bodyPartState;
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
                        if (_isTouched)
                        {
                            _isTouched = false;
                        }

                        else
                        {
                            _isTouched = true;
                        }

                        break;
                    }
                }
            }
        }
    }
}
