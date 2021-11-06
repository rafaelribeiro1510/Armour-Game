using System;
using System.Collections;
using System.Collections.Generic;
using Body.BodyType;
using Controllers;
using PartCompleteMenu;
using UnityEngine;

namespace Body
{
    public class BodyController : MonoBehaviour
    {
        public static BodyController Instance { get; private set; }
        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }

        private GameSectionController _gameSectionController;

        private BodyPartBehaviour _halfCompletePart;

        private PartCompleteScript _partCompleteMenu;
        [SerializeField] private FinishedBody finishedBody;

        private UIGlow _glow;

        private void Awake()
        {
            SingletonInitialization();
        }

        private void Start()
        {
            _glow = UIGlow.Instance;
            _partCompleteMenu = PartCompleteScript.Instance;
            _gameSectionController = GameSectionController.Instance;
        }

        private void Update()
        {
            if (finishedBody.IsFinished)
                _gameSectionController.MoveToSecondSection();
        }
 
        public bool TryPlacing(BodyPartBehaviour bodyPart)
        {
            if (bodyPart is null) return false;
            
            if (!(_halfCompletePart is null))
            {
                if (bodyPart.BodyType == _halfCompletePart.BodyType)
                {
                    _halfCompletePart.StopGlowing();
                    // Particle FX [Completing]
                    _glow.GlowSuccess();
                    _partCompleteMenu.Open();
                    GrabResultFromPartCompleteMenu();
                    return true;
                }
                else
                {
                    ReturnHalfCompletePart();
                    _glow.GlowMistake();
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
        }

        public void TryOpeningDrawer(BodyPartType drawer1Type, BodyPartType drawer2Type)
        {
            if (_halfCompletePart is null) return;
            
            if (drawer1Type != _halfCompletePart.BodyType && drawer2Type != _halfCompletePart.BodyType)
            {
                ReturnHalfCompletePart();
            }
        }
        
        private void ReturnHalfCompletePart()
        {
            // Error sound
            _glow.GlowMistake();
            _halfCompletePart.ReturnToDrawer();
            _halfCompletePart = null;
        }

        private void GrabResultFromPartCompleteMenu()
        {
            StartCoroutine(GrabResultFromPartCompleteMenu_CO());
        }
        
        private IEnumerator GrabResultFromPartCompleteMenu_CO()
        {
            while (_partCompleteMenu.Result is null) yield return null;
            
            finishedBody.InsertBodyInputInfo(_halfCompletePart.BodyType, _partCompleteMenu.Result); 
            
            _partCompleteMenu.ResetValues();
            _halfCompletePart = null;
        }
    }
}
