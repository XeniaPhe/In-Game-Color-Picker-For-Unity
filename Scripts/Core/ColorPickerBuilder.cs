#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Angles;
using Xenia.ColorPicker.Core.Data;
using Xenia.ColorPicker.Core.Parts.LowerPanel;
using Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput;
using Xenia.ColorPicker.Core.Parts.TitlePanel;
using Xenia.ColorPicker.Core.Parts.UpperPanel;
using Xenia.ColorPicker.Core.Parts.MiddlePanel;
using Xenia.ColorPicker.Utility.TransformUtility;
using Xenia.ColorPicker.Utility;
using Xenia.ColorPicker.Serialization;

namespace Xenia.ColorPicker.Core
{
    [ExecuteAlways]
    internal class ColorPickerBuilder : MonoBehaviour
    {
        [SerializeField] ColorPickerConfigurationData reversableData;
        [SerializeField] ColorPickerConfigurationData currentData;

        [SerializeField] HueDrawer hueDrawer;
        [SerializeField] Image lowerPanel;
        [SerializeField] RectTransform rhRow;
        [SerializeField] RectTransform gsRow;
        [SerializeField] RectTransform bvRow;
        [SerializeField] RectTransform aRow;
        [SerializeField] RectTransform hexadecimalRow;
        [SerializeField] RectTransform swatchesRow;
        [SerializeField] Image upperPanel;
        [SerializeField] Image pixelatedDisplay;
        [SerializeField] RectTransform memorySwatches;
        [SerializeField] Image titlePanel;
        [SerializeField] GameObject colorPickerPrefab;

        private HueSelector hueSelector;
        private SVDrawer svDrawer;
        private SVSelector svSelector;

        private Image hueDrawerImage;
        private Image hueSelectorImage;
        private Image svDrawerImage;
        private Image svSelectorImage;

        private const float LowerPanelElementHeight = 17.5f;
        private const float LowerPanelVerticalGap = 10f;
        private const float UpperPanelHeight = 50f;

        internal ColorPickerConfigurationData CurrentData => (ColorPickerConfigurationData)currentData.Clone();

        internal bool Updated => !currentData.Equals(reversableData);

        private bool mustBuildDefault = false;

        private void OnEnable()
        {
            hueSelector = hueDrawer.GetComponentInChildren<HueSelector>(true);
            svDrawer = hueDrawer.GetComponentInChildren<SVDrawer>(true);
            svSelector = svDrawer.GetComponentInChildren<SVSelector>(true);

            hueDrawerImage = hueDrawer.GetComponent<Image>();
            hueSelectorImage = hueSelector.GetComponent<Image>();
            svDrawerImage = svDrawer.GetComponent<Image>();
            svSelectorImage = svSelector.GetComponent<Image>();

            if (PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                GetComponent<NonVolatileReference>().Reset();
                mustBuildDefault = true;
            }
        }

        private void Start()
        {
            if (mustBuildDefault)
                BuildDefault();
        }

        internal void ApplyChangesAndBuild()
        {
            int hueDrawerRadius = Mathf.RoundToInt(reversableData.hueDrawerData.textureSize * reversableData.hueDrawerData.drawerSize / 2f);
            int hueDrawerInnerRadius = Mathf.RoundToInt(hueDrawerRadius * reversableData.hueDrawerData.innerCircleSize);

            bool hueDrawerChanged = !reversableData.hueDrawerData.Equals(currentData.hueDrawerData) || reversableData.hueDrawerData.drawerSize != currentData.hueDrawerData.drawerSize;
            bool hueSelectorChanged = !reversableData.hueSelectorData.Equals(currentData.hueSelectorData);
            bool svDrawerChanged = !reversableData.svDrawerData.Equals(currentData.svDrawerData);
            bool svSelectorChanged = !reversableData.svSelectorData.Equals(currentData.svSelectorData);
            bool otherSettingsChanged = !reversableData.otherData.Equals(currentData.otherData);

            if (hueDrawerChanged)
            {
                BuildHueDrawer(hueDrawerRadius, hueDrawerInnerRadius);
                BuildHueSelector(hueDrawerRadius, hueDrawerInnerRadius);
                BuildSVDrawer();
                BuildSVSelector();
            }
            else
            {
                if (hueSelectorChanged)
                {
                    BuildHueSelector(hueDrawerRadius, hueDrawerInnerRadius);
                }

                if (svDrawerChanged)
                {
                    BuildSVDrawer();
                    BuildSVSelector();
                }
                else if (svSelectorChanged)
                {
                    BuildSVSelector();
                }
            }

            if (otherSettingsChanged)
            {
                BuildLowerPanel(reversableData.otherData);
                BuildUpperPanel(reversableData.otherData);
                BuildTitlePanel(reversableData.otherData);
            }

            currentData = (ColorPickerConfigurationData)reversableData.Clone();
        }

        internal void BuildDefault()
        {
            currentData = new ColorPickerConfigurationData();
            currentData.hueDrawerData = new HueDrawerData();
            currentData.svDrawerData = new SVDrawerData();
            currentData.hueSelectorData = new HueSelectorData();
            currentData.svSelectorData = new SVSelectorData();
            currentData.otherData = new OtherData();

            reversableData = ColorPickerConfigurationData.Default;
            ApplyChangesAndBuild();
        }

        internal void ApplySavedSettings(ColorPickerConfigurationData data, Texture2D hueDrawerTexture, Texture2D hueSelectorTexture, Texture2D svSelectorTexture)
        {
            reversableData = (ColorPickerConfigurationData)data.Clone();
            currentData = (ColorPickerConfigurationData)data.Clone();

            hueDrawerImage.sprite = Sprite.Create(hueDrawerTexture, new Rect(0, 0, hueDrawerTexture.width, hueDrawerTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            hueSelectorImage.sprite = Sprite.Create(hueSelectorTexture, new Rect(0, 0, hueSelectorTexture.width, hueSelectorTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            svSelectorImage.sprite = Sprite.Create(svSelectorTexture, new Rect(0, 0, svSelectorTexture.width, svSelectorTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

            var otherData = currentData.otherData;
            BuildSVDrawer();
            BuildTitlePanel(otherData);
            BuildUpperPanel(otherData);
            BuildLowerPanel(otherData);
        }

        internal void Revert()
        {
            reversableData = (ColorPickerConfigurationData)currentData.Clone();
        }

        internal (Texture2D hueDrawerTexture, Texture2D hueSelectorTexture, Texture2D svSelectorTexture) GetTexturesDrawn()
        {
            return new(hueDrawerImage.sprite.texture, hueSelectorImage.sprite.texture, svSelectorImage.sprite.texture);
        }

        private void BuildHueDrawer(int hueDrawerRadius, int hueDrawerInnerRadius)
        {
            var hueDrawerData = reversableData.hueDrawerData;

            float center = hueDrawerData.textureSize / 2f;

            float alpha = hueDrawerData.alpha;
            float rotation = hueDrawerData.rotation;

            new CircleDrawer().DrawCircle(hueDrawerImage, hueDrawerData.textureSize, hueDrawerRadius, hueDrawerInnerRadius, GetColorOfPoint, hueDrawerData.insideColor, hueDrawerData.outsideColor, typeof(HueDrawer).Name);

            hueDrawer.Initialize(hueDrawerData.drawerSize, hueDrawerData.textureSize, hueDrawerData.innerCircleSize);

            #region Local Function

            Color GetColorOfPoint(Vector2 point)
            {
                AngleFloat angle = AngleFloat.Atan2(point.y - center, point.x - center, AngleUnit.Degrees);
                angle -= rotation;
                float hue = angle.ZeroTo360_DegreesAngle / 360f;

                Color color = Color.HSVToRGB(hue, 1f, 1f);
                color.a = alpha;
                return color;
            }

            #endregion
        }

        private void BuildSVDrawer()
        {
            CanvasGroup canvasGroup = svDrawer.GetComponent<CanvasGroup>();

            var svDrawerData = reversableData.svDrawerData;
            var hueDrawerRect = hueDrawerImage.rectTransform.rect;

            float scale = (svDrawerData.drawerSize * reversableData.hueDrawerData.innerCircleSize * reversableData.hueDrawerData.drawerSize) / Mathf.Sqrt(2f);
            Vector2 size = new Vector2(hueDrawerRect.width * scale, hueDrawerRect.height * scale);
            svDrawer.GetComponent<TransformLocker>().ChangeLock(Vector3.zero, size);

            canvasGroup.alpha = svDrawerData.alpha;

            svDrawer.Initialize(svDrawerData.textureSize);
        }

        private void BuildHueSelector(int hueDrawerRadius, int hueDrawerInnerRadius)
        {
            var hueSelectorData = reversableData.hueSelectorData;

            float ratio = reversableData.hueDrawerData.drawerSize * (hueDrawerRadius - hueDrawerInnerRadius) / (2f * hueDrawerRadius);

            var hueDrawerRect = hueDrawerImage.rectTransform.rect;
            var hueSelectorRect = hueSelectorImage.rectTransform.rect;

            Vector2 scale = new Vector2(hueDrawerRect.width / hueSelectorRect.width, hueDrawerRect.height / hueSelectorRect.height);
            scale *= ratio;
            scale *= hueSelectorData.selectorSize;

            float x = hueDrawerRect.width * ratio;
            ratio = reversableData.hueDrawerData.drawerSize * hueDrawerInnerRadius / hueDrawerRadius;
            x += hueDrawerRect.width * ratio;
            x /= 2f;

            hueSelector.GetComponent<TransformLocker>().ChangeLock(new Vector3(x, 0, 0), hueSelectorImage.rectTransform.rect.size * scale);
            hueSelector.GetComponent<Image>().color = hueSelectorData.color;

            int textureSize = hueSelectorData.textureSize;
            int selectorRadius = textureSize / 2;
            int selectorInnerCircleRadius = (int)Mathf.Round(selectorRadius * hueSelectorData.innerCircleSize);

            Color transparent = new Color(1f, 1f, 1f, 0);

            new CircleDrawer().DrawCircle(hueSelectorImage, textureSize, selectorRadius, selectorInnerCircleRadius, Color.white, transparent, transparent, typeof(HueSelector).Name);

            hueSelector.Initialize(reversableData.hueDrawerData.drawerSize, reversableData.hueDrawerData.innerCircleSize);
        }

        private void BuildSVSelector()
        {
            var svSelectorData = reversableData.svSelectorData;

            var svDrawerData = svDrawerImage.rectTransform.rect;
            var selectorRect = svSelectorImage.rectTransform.rect;

            Vector2 scale = new Vector2(svDrawerData.width / selectorRect.width, svDrawerData.height / selectorRect.height);
            scale *= svSelectorData.selectorSize;

            svSelector.GetComponent<TransformLocker>().ChangeLock(Vector3.zero, svSelectorImage.rectTransform.rect.size * scale);
            svSelector.GetComponent<Image>().color = svSelectorData.color;

            int textureSize = svSelectorData.textureSize;
            int selectorRadius = textureSize / 2;
            int selectorInnerCircleRadius = (int)Mathf.Round(selectorRadius * svSelectorData.innerCircleSize);

            Color transparent = new Color(1f, 1f, 1f, 0);

            new CircleDrawer().DrawCircle(svSelectorImage, textureSize, selectorRadius, selectorInnerCircleRadius, Color.white, transparent, transparent, typeof(SVSelector).Name);
        }

        private void BuildLowerPanel(OtherData otherData)
        {
            if (!otherData.IncludeLowerPanel)
            {
                lowerPanel.gameObject.SetActive(false);
                return;
            }

            List<RectTransform> enabledSections = new();

            ColorModeDropdown colorModeDropdown = lowerPanel.GetComponentInChildren<ColorModeDropdown>(true);

            if (otherData.includeColorInput)
            {
                int counter = 0;
                counter += otherData.includeRGB0_1 ? 1 : 0;
                counter += otherData.includeRGB0_255 ? 1 : 0;
                counter += otherData.includeHSV ? 1 : 0;

                if (counter > 1)
                {
                    enabledSections.Add(colorModeDropdown.GetComponent<RectTransform>());
                    colorModeDropdown.Initialize(otherData.includeRGB0_1, otherData.includeRGB0_255, otherData.includeHSV);
                }
                else
                {
                    colorModeDropdown.gameObject.SetActive(false);
                }

                enabledSections.Add(rhRow);
                enabledSections.Add(gsRow);
                enabledSections.Add(bvRow);

                rhRow.GetComponentInChildren<ColorSlider>(true).Initialize(otherData.colorSliderTextureSize);
                gsRow.GetComponentInChildren<ColorSlider>(true).Initialize(otherData.colorSliderTextureSize);
                bvRow.GetComponentInChildren<ColorSlider>(true).Initialize(otherData.colorSliderTextureSize);

                rhRow.GetComponentInChildren<ColorLabel>(true).Initialize(otherData.rLabelColor, otherData.hLabelColor);
                gsRow.GetComponentInChildren<ColorLabel>(true).Initialize(otherData.gLabelColor, otherData.sLabelColor);
                bvRow.GetComponentInChildren<ColorLabel>(true).Initialize(otherData.bLabelColor, otherData.vLabelColor);

                rhRow.GetComponentInChildren<TMP_Text>(true).color = otherData.rLabelColor;
                gsRow.GetComponentInChildren<TMP_Text>(true).color = otherData.gLabelColor;
                bvRow.GetComponentInChildren<TMP_Text>(true).color = otherData.bLabelColor;

                rhRow.GetComponentInChildren<PartialColorInputField>(true).gameObject.SetActive(otherData.includeInputFields);
                gsRow.GetComponentInChildren<PartialColorInputField>(true).gameObject.SetActive(otherData.includeInputFields);
                bvRow.GetComponentInChildren<PartialColorInputField>(true).gameObject.SetActive(otherData.includeInputFields);
            }
            else
            {
                colorModeDropdown.gameObject.SetActive(false);
                rhRow.gameObject.SetActive(false);
                gsRow.gameObject.SetActive(false);
                bvRow.gameObject.SetActive(false);
            }

            if (otherData.includeAlphaInput)
            {
                enabledSections.Add(aRow);
                aRow.GetComponentInChildren<ColorSlider>(true).Initialize(otherData.colorSliderTextureSize);
                aRow.GetComponentInChildren<ColorLabel>(true).Initialize(otherData.alphaLabelColor, otherData.alphaLabelColor);
                aRow.GetComponentInChildren<TMP_Text>(true).color = otherData.alphaLabelColor;
                aRow.GetComponentInChildren<PartialColorInputField>(true).gameObject.SetActive(otherData.includeInputFields);
            }
            else
            {
                aRow.gameObject.SetActive(false);
            }

            if (otherData.includeHexadecimalInput)
            {
                enabledSections.Add(hexadecimalRow);
                hexadecimalRow.GetComponentInChildren<TMP_Text>(true).color = otherData.hexadecimalLabelColor;
            }
            else
            {
                hexadecimalRow.gameObject.SetActive(false);
            }

            if (otherData.includeSwatches)
                enabledSections.Add(swatchesRow);
            else
            {
                swatchesRow.gameObject.SetActive(false);
                swatchesRow.GetComponentInChildren<ScrollRect>().GetComponent<Image>().color = otherData.lowerPanelColor;
                swatchesRow.GetComponentsInChildren<LayoutGroup>().ToList().ForEach(l => l.GetComponent<Image>().color = otherData.lowerPanelColor);
            }

            lowerPanel.color = otherData.lowerPanelColor;
            lowerPanel.gameObject.SetActive(true);

            int sectionCount = enabledSections.Count;

            lowerPanel.rectTransform.sizeDelta =
                new Vector2(lowerPanel.rectTransform.sizeDelta.x, sectionCount * (LowerPanelElementHeight + LowerPanelVerticalGap) + LowerPanelVerticalGap);

            lowerPanel.GetComponent<TransformBinder>().CorrectPosition();

            float currentVerticalPos = (lowerPanel.rectTransform.rect.height - LowerPanelElementHeight) / 2f - LowerPanelVerticalGap;

            foreach (var section in enabledSections)
            {
                section.gameObject.SetActive(true);
                section.localPosition = new Vector3(section.localPosition.x, currentVerticalPos, 0f);
                currentVerticalPos -= (LowerPanelElementHeight + LowerPanelVerticalGap);
            }
        }

        private void BuildUpperPanel(OtherData otherData)
        {
            upperPanel.gameObject.SetActive(otherData.IncludeUpperPanel);
            upperPanel.rectTransform.sizeDelta = new Vector2(upperPanel.rectTransform.sizeDelta.x, otherData.IncludeUpperPanel ? UpperPanelHeight : 0);
            upperPanel.GetComponent<TransformBinder>().CorrectPosition();
            pixelatedDisplay.gameObject.SetActive(otherData.IncludeUpperPanel && otherData.includeColorPicker);

            if (!otherData.includeColorPicker)
                return;

            upperPanel.GetComponentInChildren<ScreenColorPicker>(true).gameObject.SetActive(otherData.includeColorPicker);
            pixelatedDisplay.color = otherData.pixelatedDisplayBackgroundColor;
            memorySwatches.gameObject.SetActive(otherData.includeMemorySwatches);
            upperPanel.color = otherData.upperPanelColor;
        }

        private void BuildTitlePanel(OtherData otherData)
        {
            if (!otherData.IncludeTitlePanel)
            {
                titlePanel.gameObject.SetActive(false);
                return;
            }

            CloseButton closeButton = titlePanel.GetComponentInChildren<CloseButton>(true);
            closeButton.gameObject.SetActive(otherData.includeCloseButton);

            TMP_Text title = titlePanel.GetComponentsInChildren<TMP_Text>(true).Where(t => !t.transform.parent.Equals(closeButton.transform)).First();
            title.gameObject.SetActive(otherData.includeTitle);
            title.text = otherData.title;

            titlePanel.color = otherData.titlePanelColor;
            titlePanel.gameObject.SetActive(true);
            titlePanel.GetComponent<TransformBinder>().CorrectPosition();
        }

        [MenuItem("GameObject/Color Picker")]
        private static void InstantiateColorPicker()
        {
            string[] allPaths = AssetDatabase.GetAllAssetPaths();
            string prefabPath = allPaths.Where(p => p.EndsWith("Prefabs/Color Picker.prefab")).First();

            Object prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            Object instance = PrefabUtility.InstantiatePrefab(prefab);
        }
    }
}
#endif

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