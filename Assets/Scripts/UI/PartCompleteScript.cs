using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Body.BodyType;
using DG.Tweening;
using UI;
using UnityEngine;

public class PartCompleteScript : MonoBehaviour
{
    public static PartCompleteScript Instance { get; private set; }
    private void SingletonInitialization()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }
    
    [SerializeField] private float openTime;
    private bool _open;
    
    private Transform _transform;
    private Vector3 _closedPosition;

    private List<SizeSelector> _sizeSelectors;
    // private List<EmotionSelector> _emotionSelectors;

    public BodyInputInfo Result;

    private void Awake()
    {
        SingletonInitialization();
        _transform = GetComponent<Transform>();
        _closedPosition = _transform.position;

        _sizeSelectors = new List<SizeSelector>(GetComponentsInChildren<SizeSelector>());
        //_emotionSelectors = new List<SizeSelector>(GetComponentsInChildren<EmotionSelector>());
    }

    private void Update()
    {
        if (!_open) return;
        
        // If some selector isn't ready yet TODO emotionSelectors
        if (_sizeSelectors.Any(sizeSelector => !sizeSelector.ready)) { return; }
        
        Result = new BodyInputInfo(
            "PLACEHOLDER", "PLACEHOLDER",
            _sizeSelectors[0].output, _sizeSelectors[1].output);
        Close();
    }

    [ContextMenu("Open Size and Emotion Menu")]
    public void Open()
    {
        if (_open) return;
        _open = true;
        _transform.DOMove(Vector3.zero, openTime).SetEase(Ease.OutSine);
    }

    [ContextMenu("Close Size and Emotion Menu")]
    public void Close()
    {
        if (!_open) return;
        _open = false;
        _transform.DOMove(_closedPosition, openTime).SetEase(Ease.OutSine);
        _sizeSelectors.ForEach(x => x.ResetValues());
    }
}
