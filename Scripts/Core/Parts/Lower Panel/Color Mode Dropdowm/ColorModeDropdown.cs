using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Xenia.ColorPicker.Serialization;
using Xenia.ColorPicker.Utility;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel
{
    internal delegate void ColorModeChangedEventHandler(object sender, ColorModeChangedEventArgs e);

    [RequireComponent(typeof(TMP_Dropdown))]
    internal class ColorModeDropdown : MonoBehaviour
    {
        [SerializeField] ColorPicker colorPicker;

        [DisableInInspector][SerializeField] bool rgb0_1;
        [DisableInInspector][SerializeField] bool rgb0_255;
        [DisableInInspector][SerializeField] bool hsv;

        internal event ColorModeChangedEventHandler ColorModeChanged;

        private List<ColorMode> modes;
        private SerializableGuid reference;

        private void Awake()
        {
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
            dropdown.ClearOptions();

            modes = new List<ColorMode>();

            if (rgb0_255)
                modes.Add(ColorMode.RGB0_255);
            if (rgb0_1)
                modes.Add(ColorMode.RGB0_1);
            if (hsv)
                modes.Add(ColorMode.HSV);

            dropdown.AddOptions(modes.Select(m => m.ToString()).ToList());

            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            int value = 0;
            reference = colorPicker.GetComponent<NonVolatileReference>().Reference;

            ColorModeData data = new ColorModeData(modes[value], reference.ToString());

            if (colorPicker.SerializeState && SerializationManager.Deserialize<ColorModeData>(ref data))
                if (modes.Contains(data.ColorMode))
                    value = (int)data.ColorMode;

            GetComponent<TMP_Dropdown>().value = value;
            OnValueChanged(value);
        }

        internal void Initialize(bool rgb0_1, bool rgb0_255, bool hsv)
        {
            this.rgb0_1 = rgb0_1;
            this.rgb0_255 = rgb0_255;
            this.hsv = hsv;
        }

        private void OnValueChanged(int value)
        {
            ColorMode colorMode = modes[value];
            OnColorModeChanged(new ColorModeChangedEventArgs(colorMode));
        }

        protected virtual void OnColorModeChanged(ColorModeChangedEventArgs e)
        {
            ColorModeChanged?.Invoke(this, e);
        }

        private void OnDestroy()
        {
            SerializationManager.Serialize<ColorModeData>(new ColorModeData(modes[GetComponent<TMP_Dropdown>().value], reference.ToString()));
        }
    }
}