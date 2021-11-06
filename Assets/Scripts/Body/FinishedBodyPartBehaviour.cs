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
        private FinishedBodyPartSprites _spriteControllerPhysical;
        private FinishedBodyPartSprites _spriteControllerDisease;
        public BodyPartType BodyType;
        private BodyInputInfo _bodyInputInfo;

        private bool _isTouched = false; 
        
        private Camera _camera;
        private Collider2D _collider;
        private SpriteRenderer _renderer;

        void Awake() {
            _camera = Camera.main;
            _renderer = GetComponent<SpriteRenderer>();
            var spriteControllers = GetComponentsInChildren<FinishedBodyPartSprites>();
            _spriteControllerPhysical = spriteControllers[0];
            _spriteControllerDisease = spriteControllers[1];
        }

        public void SetBodyInputInfo(BodyInputInfo bodyInputInfo)
        {
            _bodyInputInfo = bodyInputInfo;
            
            _spriteControllerPhysical.SetSprite(BodyType, bodyInputInfo.SizePhysical);
            _spriteControllerDisease.SetSprite(BodyType, bodyInputInfo.SizeDisease);
            _collider = _spriteControllerPhysical.UpdateCollider();
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
