using DG.Tweening;
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

        private void Awake()
        {
            SingletonInitialization();
            _mainCamera = Camera.main;
        }

        private Camera _mainCamera;

        public void MoveToFirstSection()
        {
            _mainCamera.transform.DOMove(new Vector3(0, 0, -10), TransitionTime);
        }
        
        public void MoveToSecondSection()
        {
            // Slide camera
            _mainCamera.transform.DOMove(new Vector3(SecondSectionX, 0, -10), TransitionTime);
        }
    }
}
