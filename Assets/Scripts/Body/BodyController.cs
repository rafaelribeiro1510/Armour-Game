using System;
using System.Collections;
using System.Collections.Generic;
using Body.BodyType;
using Controllers;
using JetBrains.Annotations;
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
        [SerializeField] private FinishedBodiesController finishedBodies;

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
            if (finishedBodies.IsFinished)
                _gameSectionController.MoveToSecondSection();
        }
 
        public bool TryPlacing(BodyPartBehaviour bodyPart, [CanBeNull] BodyInputInfo bodyInputInfo = null)
        {
            if (bodyPart is null) return false;
            
            if (!(_halfCompletePart is null))
            {
                if (bodyPart.BodyType == _halfCompletePart.BodyType)
                {
                    _halfCompletePart.StopGlowing();
                    // Particle FX [Completing]
                    _glow.GlowSuccess();
                    if (bodyInputInfo is null)
                    {
                        _partCompleteMenu.Open();   
                    }
                    InsertBodyInputInfo(bodyInputInfo);
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

        private void InsertBodyInputInfo([CanBeNull] BodyInputInfo bodyInputInfo)
        {
            StartCoroutine(InsertBodyInputInfo_CO(bodyInputInfo));
        }
        
        private IEnumerator InsertBodyInputInfo_CO([CanBeNull] BodyInputInfo bodyInputInfo)
        {
            while (bodyInputInfo is null && _partCompleteMenu.Result is null) yield return null;
            
            finishedBodies.InsertBodyInputInfo(_halfCompletePart.BodyType, bodyInputInfo ?? _partCompleteMenu.Result); 

            _partCompleteMenu.ResetValues();
            _halfCompletePart = null;
        }
    }
}
