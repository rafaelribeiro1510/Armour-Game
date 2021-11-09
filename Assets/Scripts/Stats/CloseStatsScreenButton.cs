using UnityEngine;

namespace Stats
{
    public class CloseStatsScreenButton : MonoBehaviour
    {
        private Camera _camera;
        private Collider2D _collider;

        private StatsScreenController _controller; 

        private void Awake()
        {
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();

            _controller = GetComponentInParent<StatsScreenController>();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Ended)
                {
                    if (_collider == Physics2D.OverlapPoint(touchPos))
                    {
                        _controller.CloseScreen();
                    }
                }
            }
        }
    }
}
