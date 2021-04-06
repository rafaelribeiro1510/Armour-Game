using UnityEngine;

namespace Drawer
{
    public class WhiteBarScript : MonoBehaviour
    {
        private DrawerBehaviour _drawer;
        private float _maxScale;
        private Transform _transform;

        private void Awake()
        {
            _drawer = GetComponentInParent<DrawerBehaviour>();
            _transform = transform;
            _transform.parent = GameObject.Find("WhiteBars").transform;
            _maxScale = _transform.localScale.x;
        }

        private void Update()
        {
            var localScale = _transform.localScale;
            localScale = new Vector3(Mathf.Lerp(_maxScale, 0, _drawer.movementPercentage), localScale.y,  localScale.z);
            _transform.localScale = localScale;
        }
    }
}
