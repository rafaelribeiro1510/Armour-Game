using System;
using System.Collections.Generic;
using Body.BodyType;
using UnityEngine;

namespace Body
{
    public class FinishedBodyPartSprites : MonoBehaviour
    {
        [SerializeField] private float colliderScaleFactor;

        private SpriteRenderer _renderer;
    
        [Serializable] public struct MakeShiftDictionaryKey
        {
            public MakeShiftDictionaryKey(BodyPartType bodyPartType, BodyPartState bodyPartState)
            {
                this.bodyPartType = bodyPartType;
                this.bodyPartState = bodyPartState;
            }
            public BodyPartType bodyPartType;
            public BodyPartState bodyPartState;
        }
        [Serializable] public struct MakeShiftDictionaryEntry
        {
            public MakeShiftDictionaryKey key;
            public List<Sprite> sprites;
        }

        [SerializeField] private List<MakeShiftDictionaryEntry> spriteDictionary = new List<MakeShiftDictionaryEntry>();
        private readonly Dictionary<MakeShiftDictionaryKey, List<Sprite>> _sprites = new Dictionary<MakeShiftDictionaryKey, List<Sprite>>();

        private void Awake() 
        {
            _renderer = GetComponent<SpriteRenderer>();
            foreach (var dictEntry in spriteDictionary) _sprites.Add(dictEntry.key, dictEntry.sprites);
        }

        public void SetSprite(BodyPartType bodyPartType, BodyPartState bodyPartState, int bodyPartSize)
        {
            var key = new MakeShiftDictionaryKey(bodyPartType, bodyPartState);
            if (!_sprites.ContainsKey(key)) return;
        
            _renderer.sprite = _sprites[key][bodyPartSize];
        }

        public BoxCollider2D UpdateCollider()
        {
            var col = GetComponent<BoxCollider2D>();
            if (col != null) Destroy(col);

            col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector3(col.size.x + colliderScaleFactor, col.size.y + colliderScaleFactor, 1f);
        
            return col;
        }
    }
}
