using Controllers;
using UnityEngine;

namespace Save
{
    public class NewGameScript : MonoBehaviour
    {
        private Camera _camera;
        private Collider2D _collider;
        private GameSectionController _gameSectionController;
        
        private static bool _active = true;
        
        private void Awake()
        {
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();
        }
        
        private void Start()
        {
            _gameSectionController = GameSectionController.Instance;
        }
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Ended && _collider == Physics2D.OverlapPoint(touchPos) && _active)
                {
                    _gameSectionController.MoveToFirstSection();
                    _active = false;
                }
            }
        }
    }
}
