#if UNITY_EDITOR
using UnityEngine;

namespace Xenia.ColorPicker.Utility.TransformUtility
{
    [ExecuteAlways]
    internal class TransformBinder : TransformController
    {
        [SerializeField] TransformBinder upper;
        [SerializeField] TransformBinder lower;
        private Quaternion rotation;

        internal float Height => rectTransform.localScale.y * rectTransform.rect.height;

        protected override void OnEnable()
        {
            base.OnEnable();
            rotation = Quaternion.identity;
        }

        protected override void Update()
        {
            if (Application.isPlaying)
                return;

            rectTransform.rotation = rotation;
            rectTransform.pivot = pivot;
        }

        internal void CorrectPosition()
        {
            if (lower)
                UpdatePosition(lower);
            if (upper)
                UpdatePosition(upper);
        }

        private void UpdatePosition(TransformBinder binder)
        {
            TransformBinder binderToUpdate = upper;
            float sign = 0.5f;

            if (ReferenceEquals(binder, upper))
            {
                binderToUpdate = lower;
                sign = -0.5f;
            }

            float y = binder.position.y + sign * (binder.Height + this.Height);
            position = new Vector3(binder.position.x, y, binder.position.z);
            rectTransform.localPosition = position;

            binderToUpdate?.UpdatePosition(this);
        }
    }
}
#endif