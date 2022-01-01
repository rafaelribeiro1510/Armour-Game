using TMPro;
using UnityEngine;

namespace Save
{
    public class SaveSlotTitle : MonoBehaviour
    {
        private TextMeshPro _tmp;
        private int _saveIndex;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshPro>();
        }

        private void Start()
        {
            _saveIndex = GetComponentInParent<SaveSlot>().index;
            _tmp.text = "Save " + _saveIndex;
        }
    }
}
