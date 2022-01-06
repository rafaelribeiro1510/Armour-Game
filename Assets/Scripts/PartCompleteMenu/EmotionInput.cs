using System;
using TMPro;
using UnityEngine;

namespace PartCompleteMenu
{
    public class EmotionInput : MonoBehaviour
    {
        private enum EmotionInputType
        {
            Physical,
            Disease
        }

        private Camera _camera;
        private Collider2D _col;
        
        [SerializeField] private EmotionInputType selectorType;
        private TextMeshPro _title;
        private TextMeshPro _textBox;

        private TouchScreenKeyboard _keyboard;
        
        private bool _clicked = false;
        [HideInInspector] public bool ready;
        [HideInInspector] public string output;

        private string placeholderText = "Escreve aqui";
    
        private void Awake()
        {
            _camera = Camera.main;
            _col = GetComponent<Collider2D>();
            var textComponents = GetComponentsInChildren<TextMeshPro>(); 
            _title = textComponents[0];
            _textBox = textComponents[1];
            
            switch (selectorType)
            {
                case EmotionInputType.Physical:
                    _title.text = "Corpo";
                    break;
                case EmotionInputType.Disease:
                    _title.text = "Doença";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ResetValues();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);
                if (_col == Physics2D.OverlapPoint(touchPos))
                {
                    _keyboard = TouchScreenKeyboard.Open(_textBox.text != placeholderText ? _textBox.text : "", TouchScreenKeyboardType.Default);
                    _clicked = true;
                }
            }

            if (_keyboard != null && _clicked && (_keyboard.status == TouchScreenKeyboard.Status.Done ||
                                                  _keyboard.status == TouchScreenKeyboard.Status.LostFocus))
            {
                if (_keyboard.text == "")
                {   
                    ResetValues();
                }
                else
                {
                    _textBox.color = Color.black;
                    _textBox.text = _keyboard.text;
                    ready = true;
                    _clicked = false;
                    output = _textBox.text;   
                }
            }
        }

        public void ResetValues()
        {
            ready = false;
            _clicked = false;
            _textBox.text = placeholderText;
            _textBox.color = Color.grey;
        }
    }
}
