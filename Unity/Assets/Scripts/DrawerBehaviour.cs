using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawerBehaviour : MonoBehaviour
{
    public DrawerBehaviour pair;

    [Header("Drawer stuff")]
    
    bool isHorizontal;
    bool isLeft;
    bool isDown;
    
    static float moveRange = 2.4f;
    Vector3 startingPosition;
    Vector3 finalPosition;
    Vector3 targetPosition;

    [Header("While Being Grabbed parameters")]
    Vector2 grabPos;
    static float shrinkPercent = 1.05f;

    Collider2D col;
    bool IsGrabbed;


    private void Awake() {
        col = GetComponent<Collider2D>();

        isHorizontal = transform.rotation.z == 0;
        isLeft = transform.position.x < 0;
        isDown = transform.position.y < 0;

        startingPosition = transform.position;
        finalPosition = startingPosition + 
        new Vector3(
                    (isHorizontal?moveRange:0)*(isLeft?1:-1), 
                    (isHorizontal?0:moveRange)*(isDown?1:-1)
                    );
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
                    grabPos = touchPos;
                }
            }

            else if (touch.phase == TouchPhase.Moved && IsGrabbed){
                Debug.DrawLine(grabPos, touchPos);
                Vector2 difference = touchPos - grabPos;
                targetPosition = isHorizontal 
                ? new Vector3(transform.position.x + difference.x, transform.position.y) 
                : new Vector3(transform.position.x, transform.position.y + difference.y);

                transform.position = NormalizedWithBounds(targetPosition, startingPosition, finalPosition);
            }

            else if (touch.phase == TouchPhase.Ended){
                IsGrabbed = false;
                transform.DOScale(1, 0.1f);
            }
        }
    }

    Vector3 NormalizedWithBounds(Vector3 point, Vector3 A, Vector3 B){
        float minX = Mathf.Min(A.x, B.x);
        float minY = Mathf.Min(A.y, B.y);
        float maxX = Mathf.Max(A.x, B.x);
        float maxY = Mathf.Max(A.y, B.y);

        return new Vector3(Mathf.Clamp(point.x, minX, maxX), Mathf.Clamp(point.y, minY, maxY));
    }
}
