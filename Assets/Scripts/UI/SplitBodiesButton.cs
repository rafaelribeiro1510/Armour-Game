using UnityEngine;

namespace UI
{
    public class SplitBodiesButton : MonoBehaviour
    {
        public static SplitBodiesButton Instance { get; private set; }
        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }

        private Camera _camera;
        
        private Collider2D _collider;

        public bool isOverlapping = false;
        
        private void Awake()
        {
            SingletonInitialization();
            
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Ended && _collider == Physics2D.OverlapPoint(touchPos))
                { 
                    isOverlapping = !isOverlapping;
                }
            }
        }
    }
}
