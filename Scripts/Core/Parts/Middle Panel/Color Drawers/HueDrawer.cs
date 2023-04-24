using UnityEngine;
using Xenia.ColorPicker.Utility;

namespace Xenia.ColorPicker.Core.Parts.MiddlePanel
{
    internal class HueDrawer : ColorDrawer
    {
        [DisableInInspector][SerializeField] float innerCircleRatio;
        [DisableInInspector][SerializeField] float colorPickerSize;

        private float radius;
        private float innerCircleRadius;

        protected override void Awake()
        {
            base.Awake();
            radius = colorPickerSize * rectTransform.rect.width / 2f;
            innerCircleRadius = radius * innerCircleRatio;
        }

        internal void Initialize(float colorPickerSize, int textureSize, float innerCircleRatio)
        {
            this.colorPickerSize = colorPickerSize;
            this.textureSize = textureSize;
            this.innerCircleRatio = innerCircleRatio;
        }

        protected override bool IsPositionValid(Vector2 position)
        {
            float distance = position.magnitude;

            return distance >= innerCircleRadius && distance <= radius;
        }

        public override void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            return;
        }
    }
}