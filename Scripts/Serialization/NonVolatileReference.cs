using System;
using UnityEngine;

namespace Xenia.ColorPicker.Serialization
{
    [ExecuteAlways]
    internal class NonVolatileReference : MonoBehaviour
    {
        [SerializeField] SerializableGuid reference = SerializableGuid.Empty;

        internal SerializableGuid Reference => reference;

        private void OnEnable()
        {
            hideFlags = HideFlags.NotEditable;

            if (reference == SerializableGuid.Empty)
            {
                Reset();
            }
        }

        internal void Reset()
        {
            reference = Guid.NewGuid();
        }
    }
}
