using DG.Tweening;
using UI;
using UnityEngine;

namespace Controllers
{
    public class GameSectionController : MonoBehaviour
    {
        public static GameSectionController Instance { get; private set; }
        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }

        private const float SecondSectionX = 25;
        private const float TransitionTime = 1.5f;

        private int _currSection = 0;

        private void Awake()
        {
            SingletonInitialization();
            _mainCamera = Camera.main;
        }

        private Camera _mainCamera;

        public void MoveToFirstSection()
        {
            if (_currSection > 0) return;
            _currSection++;

            // Slide camera
            _mainCamera.transform.DOMove(new Vector3(0, 0, -10), TransitionTime);
        }
        
        public void MoveToSecondSection()
        {
            if (_currSection > 1) return;
            _currSection++;

            // Slide camera
            _mainCamera.transform.DOMove(new Vector3(SecondSectionX, 0, -10), TransitionTime);
            
            // Set Emotion Texts TODO find cleaner way of triggering this
            GameObject.Find("EmotionBoxes").GetComponent<EmotionBoxes>().SetTexts();
        }
    }
}
