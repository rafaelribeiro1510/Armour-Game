using System;
using Body.BodyType;
using TMPro;
using UnityEngine;

namespace Save
{
    public class SaveSlotText : MonoBehaviour
    {
        private TextMeshPro _attributesTMP;
        private TextMeshPro _valuesTMP;
        private SaveSlot _save;

        private void Awake()
        {
            var tmps = GetComponentsInChildren<TextMeshPro>();
            _attributesTMP = tmps[0];
            _valuesTMP = tmps[1];

        }

        private void Start()
        {
            _save = GetComponentInParent<SaveSlot>();

            var saveInfo = _save.SaveInfo;
            if (saveInfo == null)
            {
                _attributesTMP.text = "Empty";
                _valuesTMP.text = "";
            }
            else
            {
                var n = Environment.NewLine;
                var attributes =
                    "Gender:" + n +
                    "Progress: ";

                var progress = saveInfo.Count + " / 6";
            
                var values =
                    "Female"   + n +
                    progress + n;

                _attributesTMP.text = attributes;
                _valuesTMP.text = values;
            }
        }
    }
}
