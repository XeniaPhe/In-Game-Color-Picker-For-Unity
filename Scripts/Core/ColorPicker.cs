using System;
using UnityEngine;
using UnityEngine.Events;
using Xenia.ColorPicker.Core;
using System.Runtime.CompilerServices;
using Xenia.ColorPicker.Serialization;

[assembly: InternalsVisibleTo("Xenia.InGameColorPicker.Editor")]

namespace Xenia.ColorPicker
{
    [Serializable]
    public class ColorEvent : UnityEvent<Color> { }

    internal delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);
    public sealed class ColorPicker : MonoBehaviour
    {
        [Header("Serialization Settings")]
        [Tooltip("Swatches Are Always Serialized")]
        [SerializeField] bool serializeState = true;
        [SerializeField] bool serializePosition = true;
        [SerializeField] bool serializeSettings = true;
        [SerializeField] bool serializeColor = true;

        [Header("Serialized Settings")]
        [Tooltip("If Turned On, ColorChanged Event Will Only Be Fired When Color Picker Is Closed;")]
        [SerializeField] bool useColorPreview = true;
        [SerializeField] bool adjustSVSelectorColorBasedOnBrightness = true;

        [Tooltip("Draggable Only By Upper and Title Panels")]
        [SerializeField] bool draggable = true;

        [SerializeField] bool closeKeyEnabled = true;
        [SerializeField] KeyCode closeKey = KeyCode.C;

        [SerializeField] bool openKeyEnabled = true;
        [SerializeField] KeyCode openKey = KeyCode.C;

        [Tooltip("Default Color Is Applied When There's No Serialized Data Available Or When Serialization Is Off")]
        [SerializeField] Color defaultColor = Color.red;

        [Header("Nonserialized Settings")]
        [SerializeField] bool closedOnStart = true;
        [SerializeField] bool enableColorEventOnOpening = true;

        internal event ColorChangedEventHandler ColorChangedInternal;

        public ColorEvent ColorChanged;
        public ColorEvent ColorPreview;
        public UnityEvent Opening;
        public UnityEvent Closing;

        private SerializableGuid reference;
        private Vector3 position;
        private RectTransform body;
        private RectTransform canvasRect;
        private bool open;

        public bool Isopen => open;

        private HSVColor currentColor = new HSVColor(0, 1, 1);

        internal HSVColor CurrentColorHSV
        {
            get => currentColor;
            set
            {
                currentColor = value;
                OnColorChanged(new ColorChangedEventArgs(currentColor));
                OnColorChangedExternal();
            }
        }

        public Color CurrentColor
        {
            get => currentColor;
            set
            {
                currentColor = value;
                OnColorChanged(new ColorChangedEventArgs(currentColor));
            }
        }

        internal bool SerializeState => serializeState;

        public bool UseColorPreview { get => useColorPreview; set => useColorPreview = value; }
        public bool AdjustSVSelectorColorBasedOnBrightness { get => adjustSVSelectorColorBasedOnBrightness; set => adjustSVSelectorColorBasedOnBrightness = value; }
        public bool Draggable { get => draggable; set => draggable = value; }
        public bool CloseKeyEnabled { get => closeKeyEnabled; set => closeKeyEnabled = value; }
        public KeyCode CloseKey { get => closeKey; set => closeKey = value; }
        public bool OpenKeyEnabled { get => openKeyEnabled; set => openKeyEnabled = value; }
        public KeyCode OpenKey { get => openKey; set => openKey = value; }


        private void Awake()
        {
            canvasRect = GetComponent<RectTransform>();
            body = transform.GetChild(0).GetComponent<RectTransform>();
            reference = GetComponent<NonVolatileReference>().Reference;
            currentColor = defaultColor;

            if (!serializePosition && !serializeSettings && !serializeState && !serializeColor)
                return;

            ColorPickerData data = new(draggable, adjustSVSelectorColorBasedOnBrightness, closeKeyEnabled, openKeyEnabled,
                useColorPreview, closeKey, openKey, currentColor, body.position, reference.ToString());

            SerializationManager.Deserialize(ref data);

            if (serializePosition)
                body.position = data.Position;

            if (serializeColor)
                currentColor = data.CurrentColor;

            if (serializeSettings)
            {
                draggable = data.Draggable;
                adjustSVSelectorColorBasedOnBrightness = data.AdjustSVSelectorBasedOnBrightness;
                closeKeyEnabled = data.CloseKeyEnabled;
                openKeyEnabled = data.OpenKeyEnabled;
                closeKey = data.CloseKey;
                openKey = data.OpenKey;
            }
        }

        private void Start()
        {
            open = !closedOnStart;

            OnColorChanged(new ColorChangedEventArgs(currentColor));

            if (closedOnStart)
            {
                body.gameObject.SetActive(false);
                return;
            }
        }

        private void Update()
        {
            position = body.position;

            if (!open && openKeyEnabled && Input.GetKeyDown(openKey))
                Open();
            else if (open && closeKeyEnabled && Input.GetKeyDown(closeKey))
                Close();
        }

        private void OnColorChanged(ColorChangedEventArgs e)
        {
            ColorChangedInternal?.Invoke(this, e);
        }

        private void OnColorChangedExternal(bool changed = true, bool preview = true)
        {
            if (changed && (!open || !useColorPreview))
                ColorChanged?.Invoke(CurrentColor);
            if (preview && (open && useColorPreview))
                ColorPreview?.Invoke(CurrentColor);
        }

        private void OnOpening()
        {
            Opening?.Invoke();

            if (enableColorEventOnOpening)
                OnColorChangedExternal();
        }

        private void OnClosing()
        {
            Closing?.Invoke();
        }

        internal void AssignColorHSV(float h = -1f, float s = -1f, float v = -1f, float a = -1f)
        {
            currentColor.H = h;
            currentColor.S = s;
            currentColor.V = v;
            currentColor.Alpha = a;
            OnColorChanged(new ColorChangedEventArgs(currentColor));
            OnColorChangedExternal();
        }

        public void AssignColor(Color color)
        {
            currentColor = color;
            OnColorChanged(new ColorChangedEventArgs(currentColor));
        }

        public void AssignColor(float r = -1f, float g = -1f, float b = -1f, float a = -1f)
        {
            Color currentColor = CurrentColor;

            if (r < 0 || r > 1)
                r = currentColor.r;
            if (g < 0 || g > 1)
                g = currentColor.g;
            if (b < 0 || b > 1)
                b = currentColor.b;
            if (a < 0 || a > 1)
                a = currentColor.a;

            currentColor = HSVColor.FromRGB(new Color(r, g, b, a));
            OnColorChanged(new ColorChangedEventArgs(currentColor));
        }

        public void FireEvents()
        {
            OnColorChangedExternal();
        }

        public void Open()
        {
            open = true;
            body.gameObject.SetActive(true);
            OnOpening();
        }

        public void OpenAtWorldPoint(Vector3 worldPoint, Camera cam = null)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam ?? Camera.main, worldPoint);
            OpenAtScreenPoint(screenPoint);
        }

        public void OpenAtScreenPoint(Vector2 screenPoint)
        {
            Vector2 anchoredPos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out anchoredPos);
            body.anchoredPosition = anchoredPos;
            Open();
        }

        public void Close()
        {
            open = false;
            body.gameObject.SetActive(false);
            OnClosing();
            OnColorChangedExternal(useColorPreview, false);
        }

        private void OnDestroy()
        {
            ColorPickerData data = new(draggable, adjustSVSelectorColorBasedOnBrightness, closeKeyEnabled, openKeyEnabled,
                useColorPreview, closeKey, openKey, currentColor, position, reference.ToString());

            SerializationManager.Serialize(data);
        }
    }
}

//MIT License

//Copyright (c) 2023 Mustafa Tunahan BAŞ

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
