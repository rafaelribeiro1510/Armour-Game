using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class TargetBehaviour : MonoBehaviour
{
    public BodyPartBehaviour.Type BodyType = BodyPartBehaviour.Type.Torso;

    SpriteRenderer sprite;
    bool glowing = false;
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
        if (glowing) return;
        StartBlink();
    }

    public void stopGlowing(){
        if (!glowing) return;
        blinkTween.Kill(true);
    }

}
