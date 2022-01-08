using System;
using Body.BodyType;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EmotionBox : MonoBehaviour
    {
        private bool _clicked = false;
        public BodyPartType bodyPartType;
        public BodyPartState bodyPartState;
        private string _text;
        
        [SerializeField] private Color activeColor;
        [SerializeField] private Color defaultColor;
        
        private TextMeshPro _tmp;
        private SpriteRenderer _sprite;
        private Camera _camera;
        private Collider2D _collider;

        
        private const float FadeDuration = 0.5f;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshPro>();
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _camera = Camera.main;
            _collider = GetComponentInChildren<Collider2D>();
            ToggleVisible();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Ended && _collider == Physics2D.OverlapPoint(touchPos))
                { 
                    _clicked = !_clicked;
                    _tmp.text = _clicked ? _text : bodyPartType.ToString();
                    _sprite.color = _clicked ? activeColor : defaultColor;
                }
            }
        }

        public void SetText(string text)
        {
            _text = text;
            _tmp.text = bodyPartType.ToString();
            _sprite.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
        }

        public void ToggleVisible()
        {
            // Fade alpha of sprite & text
            if (_sprite.color.a > 0 && _sprite.color.a < 1) return;
            var targetA = Math.Abs(1 - _sprite.color.a);
            _sprite.DOFade(targetA, FadeDuration);
            _tmp.DOFade(targetA, FadeDuration);
        }
    }
}
