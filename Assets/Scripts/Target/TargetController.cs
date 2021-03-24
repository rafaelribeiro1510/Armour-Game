using System.Collections.Generic;
using Body.BodyType;
using UnityEngine;

namespace Target
{
    public class TargetController : MonoBehaviour
    {
        public static TargetController Instance { get; private set; }

        private Dictionary<BodyPartType, TargetState.TargetState> _targetStateMachine;
    
        private void Awake()
        {
            SingletonInitialization();

            _targetStateMachine = new Dictionary<BodyPartType, TargetState.TargetState>
            {
                {BodyPartType.Head, TargetState.TargetState.Empty},
                {BodyPartType.Torso, TargetState.TargetState.Empty},
                {BodyPartType.ArmL, TargetState.TargetState.Empty},
                {BodyPartType.ArmR, TargetState.TargetState.Empty},
                {BodyPartType.LegL, TargetState.TargetState.Empty},
                {BodyPartType.LegR, TargetState.TargetState.Empty},
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
            if (_targetStateMachine.ContainsValue(TargetState.TargetState.HalfComplete))
            {
            
            }
            else if (_targetStateMachine[target.BodyType] == TargetState.TargetState.Empty)
            {
            
            }
        }
    }
}
