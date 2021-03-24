using System;
using System.Collections.Generic;
using Body.BodyType;
using UnityEngine;

namespace Target
{
    public class TargetSprites : MonoBehaviour
    {
        private SpriteRenderer _renderer;
    
        [Serializable] public struct MakeShiftDictionaryEntry
        {
            public BodyPartType key;
            public Sprite sprite;
        }

        [SerializeField] private List<MakeShiftDictionaryEntry> spriteDictionary = new List<MakeShiftDictionaryEntry>();
        private readonly Dictionary<BodyPartType, Sprite> _sprites = new Dictionary<BodyPartType, Sprite>();

        private void Awake() 
        {
            _renderer = GetComponent<SpriteRenderer>();
            foreach (var dictEntry in spriteDictionary) _sprites.Add(dictEntry.key, dictEntry.sprite);
        }

        public void SetSprite(BodyPartType bodyPartType)
        {
            if (!_sprites.ContainsKey(bodyPartType)) return;

            _renderer.sprite = _sprites[bodyPartType];

            var col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            tag = bodyPartType.ToString();
        }
    }
}
