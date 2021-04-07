using System;
using System.Collections.Generic;
using Body.BodyType;
using UnityEngine;

namespace Body
{
    public class BodyController : MonoBehaviour
    {
        public static BodyController Instance { get; private set; }
        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }

        private BodyPartBehaviour _halfCompletePart;

        private PartCompleteScript _partCompleteMenu;
        private Dictionary<BodyPartType, BodyInputInfo> _bodyInputInfo;

        private UIGlow _glow;

        private void Awake()
        {
            SingletonInitialization();
        }

        private void Start()
        {
            _glow = UIGlow.Instance;
            _partCompleteMenu = PartCompleteScript.Instance;
        }

        public bool TryPlacing(BodyPartBehaviour bodyPart)
        {
            if (bodyPart is null) return false;
            
            if (!(_halfCompletePart is null))
            {
                if (bodyPart.BodyType == _halfCompletePart.BodyType)
                {
                    _halfCompletePart.StopGlowing();
                    _halfCompletePart = null;
                    // Particle FX [Completing]
                    _glow.GlowSuccess();
                    _partCompleteMenu.Open();
                    return true;
                }
                else
                {
                    ReturnHalfCompletePart();
                    _glow.GlowMistake();
                    return false;
                }
            }
            else
            {
                _halfCompletePart = bodyPart;
                _halfCompletePart.StartGlowing();
                // Particle FX [Half Completing]
                return true;
            }
        }

        public void TryOpeningDrawer(BodyPartType drawer1Type, BodyPartType drawer2Type)
        {
            if (_halfCompletePart is null) return;
            
            if (drawer1Type != _halfCompletePart.BodyType && drawer2Type != _halfCompletePart.BodyType)
            {
                ReturnHalfCompletePart();
            }
        }
        
        private void ReturnHalfCompletePart()
        {
            // Error sound
            _glow.GlowMistake();
            _halfCompletePart.ReturnToDrawer();
            _halfCompletePart = null;
        }
    }
}
