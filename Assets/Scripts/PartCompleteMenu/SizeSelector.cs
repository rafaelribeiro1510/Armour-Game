using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PartCompleteMenu
{
    public class SizeSelector : MonoBehaviour
    {
        private enum SizeSelectorType
        {
            Physical,
            Disease
        }

        private Camera _camera;
        
        [SerializeField] private SizeSelectorType selectorType;
        private TextMeshPro _title;

        private List<SpriteRenderer> _circleRenderers;
        private List<Collider2D> _circleCols;
        [SerializeField] private Color greyedOutColor;
        [SerializeField] private Color selectedColor;

        [HideInInspector] public bool ready;
        [HideInInspector] public int output;
    
        private void Awake()
        {
            _camera = Camera.main;
            _title = GetComponentInChildren<TextMeshPro>();
            _circleRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
            _circleCols = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
            foreach (SpriteRenderer circle in _circleRenderers) circle.color = greyedOutColor;
            
            switch (selectorType)
            {
                case SizeSelectorType.Physical:
                    _title.text = "Corpo";
                    break;
                case SizeSelectorType.Disease:
                    _title.text = "Doença";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        for (var i = 0; i < _circleCols.Count; i++)
                            if (_circleCols[i] == Physics2D.OverlapPoint(touchPos))
                                SelectSize(i);
                        
                        break;
                }
            }
        }

        private void SelectSize(int index)
        {
            if (!ready) ready = true;
            // Possible SFX
            foreach (var _renderer in _circleRenderers)
            {
                _renderer.color = greyedOutColor;
            }

            _circleRenderers[index].color = selectedColor;
            output = index;
        }

        public void ResetValues()
        {
            ready = false;
            foreach (var _renderer in _circleRenderers)
            {
                _renderer.color = greyedOutColor;
            }
        }
    }
}
