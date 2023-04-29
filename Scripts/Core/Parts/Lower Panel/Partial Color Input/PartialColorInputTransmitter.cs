using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal abstract class PartialColorInputTransmitter : MonoBehaviour, IColorChangeSubscriber
    {
        [SerializeField] ColorComponent colorComponent;
        [SerializeField] ColorPicker colorPicker;
        [SerializeField] ColorModeDropdown colorModeDropdown;

        private ColorComponentStrategy componentStrategy;
        private ColorMode colorMode;

        protected float componentValue;
        protected float componentUpperBoundary = 1f;
        protected bool wholeValues;

        protected virtual void Awake()
        {
            switch (colorComponent)
            {
                case ColorComponent.RH:
                    componentStrategy = new RHColorComponentStrategy();
                    break;
                case ColorComponent.GS:
                    componentStrategy = new GSColorComponentStrategy();
                    break;
                case ColorComponent.BV:
                    componentStrategy = new BVColorComponentStrategy();
                    break;
                case ColorComponent.A:
                    componentStrategy = new AlphaColorComponentStrategy();
                    break;
            }

            colorPicker.ColorChangedInternal += ColorChanged;
            colorModeDropdown.ColorModeChanged += ColorModeChanged;
        }

        public virtual void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            componentValue = componentStrategy.GetColorComponent(e.Color, colorMode);
        }

        protected virtual void ColorModeChanged(object sender, ColorModeChangedEventArgs e)
        {
            colorMode = e.ColorMode;
            componentUpperBoundary = componentStrategy.GetComponentUpperBoundary(colorMode);
            componentValue = componentStrategy.GetColorComponent(colorPicker.CurrentColorHSV, colorMode);

            wholeValues = colorMode == ColorMode.RGB0_255 || colorMode == ColorMode.HSV;
        }

        protected void UpdateColorPicker()
        {
            colorPicker.CurrentColorHSV = componentStrategy.ChangeComponentInTheColor(componentValue, colorPicker.CurrentColorHSV, colorMode);
        }

        protected Color GetColorIfComponentValueIs(float value)
        {
            return componentStrategy.GetColorIfComponentValueIs(value, colorPicker.CurrentColorHSV, colorMode);
        }
    }
}