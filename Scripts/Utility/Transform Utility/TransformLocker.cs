#if UNITY_EDITOR
using UnityEngine;

namespace Xenia.ColorPicker.Utility.TransformUtility
{
    [ExecuteAlways]
    internal class TransformLocker : TransformController
    {
        private Vector2 scale;
        private Vector2 size;

        protected override void OnEnable()
        {
            base.OnEnable();
            scale = rectTransform.localScale;
            size = rectTransform.rect.size;
            hideFlags = HideFlags.None;
        }

        protected override void Update()
        {
            if (Application.isPlaying)
                return;

            rectTransform.localPosition = position;
            rectTransform.localScale = scale;
            rectTransform.rotation = Quaternion.identity;
            rectTransform.pivot = pivot;

            SetSize();
        }

        internal void ChangeLock(Vector3 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }

        private void SetSize()
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}
#endif
