#if UNITY_EDITOR
using UnityEngine;

namespace Xenia.ColorPicker.Utility.TransformUtility
{
    [ExecuteAlways]
    internal abstract class TransformController : MonoBehaviour
    {
        protected RectTransform rectTransform;

        protected Vector3 position;
        protected Vector2 pivot;

        protected virtual void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            position = rectTransform.localPosition;
            pivot = new Vector2(0.5f, 0.5f);
        }

        protected abstract void Update();
    }
}
#endif