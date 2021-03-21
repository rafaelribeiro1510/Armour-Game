using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class TargetBehaviour : MonoBehaviour
{
    public BodyPartType BodyType;
    private TargetSprites spriteController;

    SpriteRenderer _renderer;
    Color originalColor;

    [SerializeField]
    float blinkDuration;
    Sequence blinkTween;

    void StartBlink() {
        blinkTween = DOTween.Sequence();
        blinkTween.Append(_renderer.DOColor(new Color(1,1,1,0.5f), blinkDuration)).Append(_renderer.DOColor(originalColor, blinkDuration)).SetLoops(Int32.MaxValue);
    }

    private void Awake() {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = _renderer.color;
        
        spriteController = GetComponentInChildren<TargetSprites>();
    }

    private void Start()
    {
        spriteController.SetSprite(BodyType);
    }

    public void startGlowing(){
        StartBlink();
    }

    public void stopGlowing(){
        blinkTween.Kill(true);
    }

}
