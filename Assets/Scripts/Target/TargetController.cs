using System.Collections.Generic;
using System.Linq;
using Body;
using Body.BodyType;
using UnityEngine;

namespace Target
{
    public class TargetController : MonoBehaviour
    {
        public static TargetController Instance { get; private set; }

        private Dictionary<BodyPartType, TargetState.TargetState> _targetStateMachine;
        private BodyPartBehaviour _halfCompletePart;

        private void Awake()
        {
            SingletonInitialization();

            _targetStateMachine = new Dictionary<BodyPartType, TargetState.TargetState>
            {
                {BodyPartType.Head, TargetState.TargetState.Empty },
                {BodyPartType.Torso, TargetState.TargetState.Empty},
                {BodyPartType.ArmL, TargetState.TargetState.Empty },
                {BodyPartType.ArmR, TargetState.TargetState.Empty },
                {BodyPartType.LegL, TargetState.TargetState.Empty },
                {BodyPartType.LegR, TargetState.TargetState.Empty },
            };
        }

        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }

        public bool TryPlacing(BodyPartBehaviour bodyPart)
        {
            if (bodyPart is null) return false;
            
            if (!(_halfCompletePart is null))
            {
                if (bodyPart.BodyType == _halfCompletePart.BodyType)
                {
                    _targetStateMachine[_halfCompletePart.BodyType] = TargetState.TargetState.Complete;
                    // Particle FX [Completing]
                }
                else
                {
                    ReturnHalfCompletePart();
                    bodyPart.ReturnToDrawer();
                    return false;
                }
            }
            else
            {
                _halfCompletePart = bodyPart;
                _targetStateMachine[_halfCompletePart.BodyType] = TargetState.TargetState.HalfComplete;
            }
            
            // Successfully places part
            
            // Particle FX [Half Completing]
            return true;
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
            // Screen flash red / Camera shake
            _halfCompletePart.ReturnToDrawer();
        }
    }
}
