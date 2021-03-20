using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class TargetBehaviour : MonoBehaviour
{
    public BodyPartType BodyType;

    SpriteRenderer sprite;
    Color ogColor;

    [SerializeField]
    float blinkDuration;
    Sequence blinkTween;

    void StartBlink() {
        blinkTween = DOTween.Sequence();
        blinkTween.Append(sprite.DOColor(new Color(1,1,1,0.5f), blinkDuration)).Append(sprite.DOColor(ogColor, blinkDuration)).SetLoops(Int32.MaxValue);
    }

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        ogColor = sprite.color;
    }

    public void startGlowing(){
        StartBlink();
    }

    public void stopGlowing(){
        blinkTween.Kill(true);
    }

}
