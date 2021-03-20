using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBarScript : MonoBehaviour
{
    private SingleDrawerBehaviour drawer;
    private float maxScale;
    private Transform _transform;
    
    void Awake()
    {
        drawer = GetComponentInParent<SingleDrawerBehaviour>();
        _transform = transform;
        _transform.parent = null;
        maxScale = _transform.localScale.y;
    }

    void Update()
    {
        _transform.localScale = new Vector3(_transform.localScale.x, Mathf.Lerp(maxScale, 0, drawer._movementPercentage), _transform.localScale.z);
    }
}
