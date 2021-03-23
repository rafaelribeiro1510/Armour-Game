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
        _transform.parent = GameObject.Find("WhiteBars").transform;
        maxScale = _transform.localScale.x;
    }

    void Update()
    {
        _transform.localScale = new Vector3(Mathf.Lerp(maxScale, 0, drawer._movementPercentage), _transform.localScale.y,  _transform.localScale.z);
    }
}
