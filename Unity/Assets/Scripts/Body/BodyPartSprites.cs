using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BodyPartSprites : MonoBehaviour
{
    [SerializeField] private float colliderScaleFactor;
    
    private SpriteRenderer _renderer;
    
    [Serializable] public struct MakeShiftDictionaryEntry
    {
        public BodyPartType key;
        public List<Sprite> sprites;
    }

    [SerializeField] private List<MakeShiftDictionaryEntry> spriteDictionary = new List<MakeShiftDictionaryEntry>();
    Dictionary<BodyPartType, List<Sprite>> _sprites = new Dictionary<BodyPartType, List<Sprite>>();
    
    void Awake() 
    {
        _renderer = GetComponent<SpriteRenderer>();
        foreach (var dictEntry in spriteDictionary) _sprites.Add(dictEntry.key, dictEntry.sprites);
    }

    public void SetSprite(BodyPartType bodyPartType, BodyPartState bodyPartState)
    {
        if (!_sprites.ContainsKey(bodyPartType)) return;
        
        _renderer.sprite = _sprites[bodyPartType][bodyPartState == BodyPartState.Outline ? 0 : 1];
    }

    public BoxCollider2D UpdateCollider()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) Destroy(col);

        col = gameObject.AddComponent<BoxCollider2D>();
        col.size = new Vector3(col.size.x + colliderScaleFactor, col.size.y + colliderScaleFactor, 1f);
        
        return col;
    }
}
