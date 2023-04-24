using System.Collections.Generic;
using System.Linq;
using Xenia.ColorPicker.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class SwatchController : MonoBehaviour
    {
        [SerializeField] ColorPicker colorPicker;
        [SerializeField] SwatchOperationsPanel operationsPanel;

        [SerializeField] ColorSwatch swatchPrefab;
        [SerializeField] ColorSwatchAdder swatchAdderPrefab;

        [SerializeField] ScrollRect scrollRect;
        [SerializeField] RectTransform listLayout;
        [SerializeField] RectTransform gridLayout;
        [SerializeField] RectTransform scrollUI;

        private List<ColorSwatch> swatches;
        private Image scrollRectImage;
        private RectMask2D mask;
        private ColorSwatchAdder swatchAdder;
        private RectTransform layout;
        private Vector3 listPos, gridPos;
        private SwatchLayout currentLayout = SwatchLayout.GridLayout;
        private SerializableGuid reference;
        internal SwatchLayout CurrentLayout => currentLayout;
        internal SwatchOperationsPanel OperationsPanel => operationsPanel;

        private void Awake()
        {
            scrollRectImage = scrollRect.GetComponent<Image>();
            mask = scrollRect.GetComponentInChildren<RectMask2D>();

            listPos = listLayout.localPosition;
            gridPos = gridLayout.localPosition;

            IEnumerable<Color> colors = new List<Color>();

            reference = colorPicker.GetComponent<NonVolatileReference>().Reference;

            SwatchData data = new SwatchData(SwatchLayout.GridLayout, false,
                colors, reference.ToString());

            SerializationManager.Deserialize<SwatchData>(ref data);

            if(colorPicker.SerializeState)
                currentLayout = data.Layout;

            colors = data.Swatches;
            layout = currentLayout == SwatchLayout.GridLayout ? gridLayout : listLayout;

            swatches = new List<ColorSwatch>(colors.Count());

            ChangeLayout(currentLayout);

            swatchAdder = Instantiate<ColorSwatchAdder>(swatchAdderPrefab, layout);
            swatchAdder.Initialize(this, colorPicker);

            foreach (var color in colors)
                AddSwatch(color);

            ReturnToInitialPosition();
            Toggle(colorPicker.SerializeState ? data.SwatchPanelOn : false);
        }

        private void ReturnToInitialPosition()
        {
            layout.localPosition = currentLayout == SwatchLayout.ListLayout ? listPos : gridPos;
        }

        private void UpdateScrollUI()
        {
            int swatchCount = swatches.Count;
            bool enabled = false;

            switch (currentLayout)
            {
                case SwatchLayout.ListLayout:
                    enabled = swatchCount > 13;
                    break;
                case SwatchLayout.GridLayout:
                    enabled = swatchCount > 271 || (mask.enabled && swatchCount > 254);
                    break;
            }

            if (mask.enabled == enabled)
                return;

            scrollRectImage.enabled = enabled;
            scrollUI.gameObject.SetActive(enabled);
            scrollRect.vertical = enabled;
            mask.enabled = enabled;

            if (!enabled)
                ReturnToInitialPosition();

            if (currentLayout == SwatchLayout.GridLayout)
            {
                GridLayoutGroup group = gridLayout.GetComponent<GridLayoutGroup>();
                RectOffset padding = new RectOffset
                {
                    bottom = group.padding.bottom,
                    top = group.padding.top,
                    left = group.padding.left,
                    right = enabled ? 20 : 0
                };

                group.padding = padding;
            }
        }

        internal void Toggle(bool enabled)
        {
            scrollRect.gameObject.SetActive(enabled);
        }

        internal bool IsEnabled()
        {
            return scrollRect.gameObject.activeSelf;
        }

        internal void ChangeLayout(SwatchLayout newLayout)
        {
            layout.gameObject.SetActive(false);
            currentLayout = newLayout;
            layout = currentLayout == SwatchLayout.ListLayout ? listLayout : gridLayout;
            layout.gameObject.SetActive(true);

            scrollRect.content = layout;

            swatches.ForEach(s => s.transform.SetParent(layout));
            swatchAdder?.transform.SetParent(layout);

            UpdateScrollUI();
        }

        internal void AddSwatch(Color color)
        {
            swatchAdder.transform.SetParent(null, true);

            ColorSwatch instance = Instantiate<ColorSwatch>(swatchPrefab, layout);
            instance.Initialize(this, color);
            swatches.Add(instance);

            swatchAdder.transform.SetParent(layout);

            UpdateScrollUI();
        }

        internal void RemoveSwatch(ColorSwatch swatch)
        {
            int removeIndex = swatches.IndexOf(swatch);
            swatches.RemoveAt(removeIndex);
            swatch.transform.SetParent(null);
            Destroy(swatch.gameObject);

            UpdateScrollUI();
        }

        internal void UpdateSwatch(ColorSwatch swatch)
        {
            swatch.Color = colorPicker.CurrentColor;
        }

        internal void MoveSwatchToFirst(ColorSwatch swatch)
        {
            if (swatches.Count < 2)
                return;

            swatch.transform.SetAsFirstSibling();

            for (int i = swatches.IndexOf(swatch); i > 0; --i)
                swatches[i] = swatches[i - 1];

            swatches[0] = swatch;
        }

        internal void UpdateColorPicker(Color color)
        {
            colorPicker.CurrentColorHSV = color;
        }

        private void OnDestroy()
        {
            SwatchData data = new SwatchData(currentLayout, IsEnabled(),
                swatches.Select(s => s.Color), reference.ToString());
            SerializationManager.Serialize<SwatchData>(data);
        }
    }
}