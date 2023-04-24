using UnityEngine;
using Xenia.ColorPicker.Utility;
using Angles;

namespace Xenia.ColorPicker.Core.Parts.MiddlePanel
{
    internal class HueSelector : ColorSelector
    {
        [SerializeField] SVDrawer svDrawer;

        [DisableInInspector][SerializeField] float selectorDistanceRatio;
        [DisableInInspector][SerializeField] float colorPickerSize;

        private float distanceFromCenter;
        private float hue;

        protected override void Awake()
        {
            base.Awake();
            distanceFromCenter = colorPickerSize * targetDrawer.rect.width * selectorDistanceRatio / 2f;
        }

        internal void Initialize(float colorPickerSize, float innerCircleRatio)
        {
            this.colorPickerSize = colorPickerSize;
            selectorDistanceRatio = (innerCircleRatio + 1f) / 2f;
        }

        protected override void CorrectPosition(Vector3 position)
        {
            rectTransform.localPosition = position.normalized * distanceFromCenter;
            AngleFloat angle = AngleFloat.Atan2(position.y, position.x, AngleUnit.Degrees);
            hue = angle.ZeroTo360_DegreesAngle / 360f;
        }

        protected override void UpdateColorPicker()
        {
            colorPicker.AssignColorHSV(h: hue);
        }

        public override void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            hue = e.Color.H * 360f * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3(Mathf.Cos(hue), Mathf.Sin(hue), 0f);
            CorrectPosition(newPos);
        }
    }
}