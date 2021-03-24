using System.Collections.Generic;
using Body.BodyType;
using UnityEngine;

namespace Target
{
    public class TargetController : MonoBehaviour
    {
        public static TargetController Instance { get; private set; }

        private Dictionary<BodyPartType, TargetState> _targetStateMachine;
    
        private void Awake()
        {
            SingletonInitialization();

            _targetStateMachine = new Dictionary<BodyPartType, TargetState>
            {
                {BodyPartType.Head, TargetState.Empty},
                {BodyPartType.Torso, TargetState.Empty},
                {BodyPartType.ArmL, TargetState.Empty},
                {BodyPartType.ArmR, TargetState.Empty},
                {BodyPartType.LegL, TargetState.Empty},
                {BodyPartType.LegR, TargetState.Empty},
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

        private void TryPlacing(TargetBehaviour target)
        {
            if (_targetStateMachine.ContainsValue(TargetState.HalfComplete))
            {
            
            }
            else if (_targetStateMachine[target.BodyType] == TargetState.Empty)
            {
            
            }
        }
    }
}
