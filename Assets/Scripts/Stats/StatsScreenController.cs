using Body.BodyType;
using DG.Tweening;
using UnityEngine;

namespace Stats
{
    public class StatsScreenController : MonoBehaviour
    {
        public static bool _awakened;
        public static StatsScreenController Instance { get; private set; }
        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }

            _awakened = true;
        }

        private BodyPartType? _clickedBodyPart;
        
        private const float MovingDuration = 0.5f;
        private const float MovingDistance = 11;
        
        private void Awake()
        {
            SingletonInitialization();
        }

        [ContextMenu("Close Stats Screen")]
        public void CloseScreen()
        {
            if (_clickedBodyPart != null)
            {
                transform.DOMoveY(-MovingDistance, MovingDuration).SetRelative();
                _clickedBodyPart = null;
            }
        }

        public void SetBodyType(BodyPartType? bodyPartType)
        {
            if (!bodyPartType.HasValue)
            {
                _clickedBodyPart = null;
            }
            else
            {
                _clickedBodyPart = bodyPartType;
                transform.DOMoveY(MovingDistance, MovingDuration).SetRelative();
            }   
        }
    }
}
