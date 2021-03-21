using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSprites : MonoBehaviour
{
    private SpriteRenderer _renderer;
    
    [Serializable] public struct MakeShiftDictionaryEntry
    {
        public BodyPartType key;
        public Sprite sprite;
    }

    [SerializeField] private List<MakeShiftDictionaryEntry> spriteDictionary = new List<MakeShiftDictionaryEntry>();
    Dictionary<BodyPartType, Sprite> _sprites = new Dictionary<BodyPartType, Sprite>();
    
    void Awake() 
    {
        _renderer = GetComponent<SpriteRenderer>();
        foreach (var dictEntry in spriteDictionary) _sprites.Add(dictEntry.key, dictEntry.sprite);
    }

    public void SetSprite(BodyPartType bodyPartType)
    {
        if (!_sprites.ContainsKey(bodyPartType)) return;

        _renderer.sprite = _sprites[bodyPartType];

        BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        tag = bodyPartType.ToString();
    }
}
