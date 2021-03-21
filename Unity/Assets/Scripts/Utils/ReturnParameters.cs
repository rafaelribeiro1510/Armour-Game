using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "ReturnParameters", menuName = "ScriptableObjects/ReturnParameters", order = 1)]
public class ReturnParameters : ScriptableObject
{
    public float returnDuration;
    public Ease returnEase = Ease.Linear;
}
