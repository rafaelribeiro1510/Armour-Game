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
        [SerializeField] private FinishedBody finishedBodyPhysical;
        [SerializeField] private FinishedBody finishedBodyDisease;

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
            if (finishedBodyPhysical.IsFinished)
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
                    InsertBodyInputInfo();
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

        private void InsertBodyInputInfo()
        {
            StartCoroutine(InsertBodyInputInfo_CO());
        }
        
        private IEnumerator InsertBodyInputInfo_CO()
        {
            while (_partCompleteMenu.Result is null) yield return null;
            
            finishedBodyPhysical.InsertBodyInputInfo(_halfCompletePart.BodyType, _partCompleteMenu.Result); 
            finishedBodyDisease.InsertBodyInputInfo(_halfCompletePart.BodyType, _partCompleteMenu.Result);
            
            _partCompleteMenu.ResetValues();
            _halfCompletePart = null;
        }
    }
}
