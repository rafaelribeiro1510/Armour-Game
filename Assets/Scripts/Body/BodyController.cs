using Body.BodyType;
using UnityEngine;

namespace Body
{
    public class BodyController : MonoBehaviour
    {
        public static BodyController Instance { get; private set; }

        [SerializeField] private BodyPartBehaviour _halfCompletePart;

        private void Awake()
        {
            SingletonInitialization();
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
                    _halfCompletePart.StopGlowing();
                    _halfCompletePart = null;
                    // Particle FX [Completing]
                }
                else
                {
                    ReturnHalfCompletePart();
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

            return false;
        }

        public void TryOpeningDrawer(BodyPartType drawer1Type, BodyPartType drawer2Type)
        {
            if (_halfCompletePart is null) return;
            
            if (drawer1Type != _halfCompletePart.BodyType && drawer2Type != _halfCompletePart.BodyType)
            {
                print("Wrong type ; returning");
                ReturnHalfCompletePart();
            }
            else print("Right type, not returning");
        }
        
        private void ReturnHalfCompletePart()
        {
            // Error sound
            // Screen flash red / Camera shake
            _halfCompletePart.ReturnToDrawer();
            _halfCompletePart = null;
        }
    }
}
