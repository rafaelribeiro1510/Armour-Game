using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BodyPartBehaviour : MonoBehaviour
{
    public enum Type{
        Head,
        Torso,
        LArm,
        RArm,
        LLeg,
        RLeg
    }
    
    [SerializeField]
    Type BodyType = Type.Torso;

    [Header("Return parameters")]
    [SerializeField]
    float returnDuration;
    [SerializeField]
    Ease returnEase = Ease.Linear;
    
    [Header("While Being Grabbed parameters")]
    [SerializeField]
    float shrinkPercent;

    bool IsGrabbed;
    bool wrongPosition = true;
    
    Collider2D col;
    Vector3 initialPos;

    TargetBehaviour currHovering;
    
    void Awake() {
        col = GetComponent<Collider2D>();    
        initialPos = transform.position;
    }

    void Update()
    {
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began){
                if (col == Physics2D.OverlapPoint(touchPos)){
                    IsGrabbed = true;
                    transform.DOScale(shrinkPercent, 0.1f);
                }
            }

            else if (touch.phase == TouchPhase.Moved && IsGrabbed){
                transform.position = touchPos;
            }

            else if (touch.phase == TouchPhase.Ended){
                IsGrabbed = false;
                transform.DOScale(1, 0.1f);

                if (transform.position != initialPos){
                    if (wrongPosition)  
                        transform.DOMove(initialPos, returnDuration).SetEase(returnEase);
                    else{
                        transform.DOMove(currHovering.transform.position, returnDuration/2).SetEase(returnEase);
                        currHovering.stopGlowing();
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Target")){
            if (currHovering == null) currHovering = other.GetComponent<TargetBehaviour>();
            else print("Uhhh currHovering aint null cuh");

            if (currHovering.BodyType == this.BodyType){
                wrongPosition = false;
                currHovering.startGlowing();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Target")){
            wrongPosition = true;
            currHovering.stopGlowing();
            currHovering = null;
        }
    }
}
