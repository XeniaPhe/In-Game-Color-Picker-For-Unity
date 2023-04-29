using UnityEngine;
using System.Collections.Generic;
using Xenia.ColorPicker.Core.Data;
using UnityEditor;

namespace Xenia.ColorPicker.EditorScripts
{
    [CustomPropertyDrawer(typeof(ColorPickerConfigurationData))]
    internal class ColorPickerDataDrawer : PropertyDrawer
    {
        private const float VerticalPadding = 4f;

        private static bool hueDrawerFoldout = true;
        private static bool hueSelectorFoldout = true;
        private static bool svDrawerFoldout = true;
        private static bool svSelectorFoldout = true;
        private static bool otherSettingsFoldout = true;

        private bool rgb255, rgb1, hsv;
        private float unitHeight = EditorGUIUtility.singleLineHeight + VerticalPadding;
        private Rect positionRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 5.3f * EditorGUIUtility.singleLineHeight + VerticalPadding;
            int numberOfRows = 0;

            if (hueDrawerFoldout)
                numberOfRows += 6;

            if (hueSelectorFoldout)
                numberOfRows += 4;

            if (svDrawerFoldout)
                numberOfRows += 3;

            if (svSelectorFoldout)
                numberOfRows += 4;

            if (otherSettingsFoldout)
                numberOfRows += GetOtherSettingsHeight(property);

            return height + numberOfRows * unitHeight;
        }

        private void UpdateVerticalPosition()
        {
            positionRect.height = EditorGUIUtility.singleLineHeight;
            positionRect.y += (positionRect.height + VerticalPadding);
        }

        private void DrawConsequentProperties(List<SerializedProperty> consequentProps)
        {
            foreach (var prop in consequentProps)
            {
                UpdateVerticalPosition();
                EditorGUI.PropertyField(positionRect, prop);
            }
        }

        void DrawConsequentToggles(List<SerializedProperty> consequentProps)
        {
            foreach (var prop in consequentProps)
            {
                UpdateVerticalPosition();
                prop.boolValue = EditorGUI.ToggleLeft(positionRect, prop.displayName, prop.boolValue);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            positionRect = position;

            EditorGUI.BeginProperty(positionRect, label, property);
            EditorGUI.indentLevel++;

            positionRect.height = EditorGUIUtility.singleLineHeight;

            hueDrawerFoldout = EditorGUI.Foldout(positionRect, hueDrawerFoldout, new GUIContent("Hue Drawer Settings"), true);

            if (hueDrawerFoldout)
            {
                DrawHueDrawerFoldout(property.FindPropertyRelative("hueDrawerData"));
            }

            UpdateVerticalPosition();
            hueSelectorFoldout = EditorGUI.Foldout(positionRect, hueSelectorFoldout, new GUIContent("Hue Selector Settings"), true);

            if (hueSelectorFoldout)
            {
                DrawHueSelectorFoldout(property.FindPropertyRelative("hueSelectorData"));
            }

            UpdateVerticalPosition();
            svDrawerFoldout = EditorGUI.Foldout(positionRect, svDrawerFoldout, new GUIContent("SV Drawer Settings"), true);

            if (svDrawerFoldout)
            {
                DrawSVDrawerFoldout(property.FindPropertyRelative("svDrawerData"));
            }

            UpdateVerticalPosition();
            svSelectorFoldout = EditorGUI.Foldout(positionRect, svSelectorFoldout, new GUIContent("SV Selector Settings"), true);

            if (svSelectorFoldout)
            {
                DrawSVSelectorFoldout(property.FindPropertyRelative("svSelectorData"));
            }


            UpdateVerticalPosition();
            otherSettingsFoldout = EditorGUI.Foldout(positionRect, otherSettingsFoldout, new GUIContent("Other Settings"), true);

            if (otherSettingsFoldout)
            {
                DrawOtherSettingsFoldout(property.FindPropertyRelative("otherData"));
            }

            EditorGUI.EndProperty();
        }

        private void DrawHueDrawerFoldout(SerializedProperty hueDrawerData)
        {
            EditorGUI.indentLevel++;

            List<SerializedProperty> consequentProps = new List<SerializedProperty>
            {
                hueDrawerData.FindPropertyRelative("textureSize"),
                hueDrawerData.FindPropertyRelative("drawerSize"),
                hueDrawerData.FindPropertyRelative("innerCircleSize"),
                hueDrawerData.FindPropertyRelative("rotation"),
                hueDrawerData.FindPropertyRelative("alpha")
            };

            DrawConsequentProperties(consequentProps);
            UpdateVerticalPosition();

            Rect rect = EditorGUI.PrefixLabel(positionRect, EditorGUIUtility.GetControlID(FocusType.Passive),
                        new GUIContent("Hue Drawer Colors :", "Outside and inside colors of the hue drawer"));
            int originalIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float originalLabelWidth = EditorGUIUtility.labelWidth;
            float padding = 8f;
            float width = (rect.width = (rect.width - padding) / 2f);
            EditorGUIUtility.labelWidth = 50f;

            SerializedProperty outsideColor = hueDrawerData.FindPropertyRelative("outsideColor");
            SerializedProperty insideColor = hueDrawerData.FindPropertyRelative("insideColor");

            EditorGUI.PropertyField(rect, outsideColor, new GUIContent("Outside"));
            EditorGUIUtility.labelWidth = 40f;
            rect.x += (width + padding);
            EditorGUI.PropertyField(rect, insideColor, new GUIContent("Inside"));

            EditorGUI.indentLevel = originalIndent;
            EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUI.indentLevel--;
        }

        private void DrawHueSelectorFoldout(SerializedProperty hueSelectorData)
        {
            EditorGUI.indentLevel++;

            List<SerializedProperty> consequentProps = new List<SerializedProperty>
            {
                hueSelectorData.FindPropertyRelative("textureSize"),
                hueSelectorData.FindPropertyRelative("selectorSize"),
                hueSelectorData.FindPropertyRelative("innerCircleSize"),
                hueSelectorData.FindPropertyRelative("color")
            };

            DrawConsequentProperties(consequentProps);

            EditorGUI.indentLevel--;
        }

        private void DrawSVDrawerFoldout(SerializedProperty svDrawerData)
        {
            EditorGUI.indentLevel++;

            List<SerializedProperty> consequentProps = new List<SerializedProperty>
            {
                svDrawerData.FindPropertyRelative("textureSize"),
                svDrawerData.FindPropertyRelative("drawerSize"),
                svDrawerData.FindPropertyRelative("alpha")
            };

            DrawConsequentProperties(consequentProps);

            EditorGUI.indentLevel--;
        }

        private void DrawSVSelectorFoldout(SerializedProperty svSelectorData)
        {
            EditorGUI.indentLevel++;

            List<SerializedProperty> consequentProps = new List<SerializedProperty>
            {
                svSelectorData.FindPropertyRelative("textureSize"),
                svSelectorData.FindPropertyRelative("selectorSize"),
                svSelectorData.FindPropertyRelative("innerCircleSize"),
                svSelectorData.FindPropertyRelative("color")
            };

            DrawConsequentProperties(consequentProps);

            EditorGUI.indentLevel--;
        }

        private void DrawOtherSettingsFoldout(SerializedProperty otherData)
        {
            EditorGUI.indentLevel++;

            SerializedProperty includeLowerPanel = otherData.FindPropertyRelative("includeLowerPanel");

            UpdateVerticalPosition();
            bool guiEnabled = includeLowerPanel.boolValue = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include Lower Panel"), includeLowerPanel.boolValue);
            EditorGUI.Foldout(positionRect, guiEnabled, GUIContent.none, true);

            if (guiEnabled)
            {
                EditorGUI.indentLevel++;

                SerializedProperty includeSwatches = otherData.FindPropertyRelative("includeSwatches");
                SerializedProperty includeHexadecimalInput = otherData.FindPropertyRelative("includeHexadecimalInput");
                SerializedProperty includeAlphaInput = otherData.FindPropertyRelative("includeAlphaInput");
                SerializedProperty includeColorInput = otherData.FindPropertyRelative("includeColorInput");

                UpdateVerticalPosition();
                EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("lowerPanelColor"));

                UpdateVerticalPosition();
                includeSwatches.boolValue = EditorGUI.ToggleLeft(positionRect, includeSwatches.displayName, includeSwatches.boolValue);

                UpdateVerticalPosition();
                guiEnabled = includeHexadecimalInput.boolValue = EditorGUI.ToggleLeft(positionRect, includeHexadecimalInput.displayName, includeHexadecimalInput.boolValue);

                if (guiEnabled)
                {
                    EditorGUI.indentLevel++;
                    UpdateVerticalPosition();
                    EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("hexadecimalLabelColor"));
                    EditorGUI.indentLevel--;
                }

                UpdateVerticalPosition();
                guiEnabled = includeAlphaInput.boolValue = EditorGUI.ToggleLeft(positionRect, includeAlphaInput.displayName, includeAlphaInput.boolValue);

                if (guiEnabled)
                {
                    EditorGUI.indentLevel++;
                    UpdateVerticalPosition();
                    EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("alphaLabelColor"));
                    EditorGUI.indentLevel--;
                }

                UpdateVerticalPosition();

                guiEnabled = includeColorInput.boolValue = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include Color Input"), includeColorInput.boolValue);
                EditorGUI.Foldout(positionRect, guiEnabled, GUIContent.none, true);

                if (guiEnabled)
                {
                    EditorGUI.indentLevel++;

                    SerializedProperty includeRGB0_255 = otherData.FindPropertyRelative("includeRGB0_255");
                    SerializedProperty includeRGB0_1 = otherData.FindPropertyRelative("includeRGB0_1");

                    guiEnabled = false;
                    UpdateVerticalPosition();
                    includeRGB0_255.boolValue = rgb255 = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include RGB(0-255)"), includeRGB0_255.boolValue);

                    if (!rgb1 && !hsv)
                        includeRGB0_255.boolValue = rgb255 = true;

                    guiEnabled |= rgb255;

                    UpdateVerticalPosition();
                    includeRGB0_1.boolValue = rgb1 = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include RGB(0-1.0)"), includeRGB0_1.boolValue);

                    if (!rgb255 && !hsv)
                        includeRGB0_1.boolValue = rgb1 = true;

                    guiEnabled |= rgb1;

                    if (guiEnabled)
                    {
                        EditorGUI.indentLevel++;

                        var consequentProps = new List<SerializedProperty>
                        {
                            otherData.FindPropertyRelative("rLabelColor"),
                            otherData.FindPropertyRelative("gLabelColor"),
                            otherData.FindPropertyRelative("bLabelColor")
                        };

                        DrawConsequentProperties(consequentProps);

                        EditorGUI.indentLevel--;
                    }

                    SerializedProperty includeHSV = otherData.FindPropertyRelative("includeHSV");

                    UpdateVerticalPosition();
                    includeHSV.boolValue = hsv = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include HSV"), includeHSV.boolValue);

                    if (!rgb255 && !rgb1)
                        includeHSV.boolValue = hsv = true;

                    guiEnabled = hsv;

                    if (guiEnabled)
                    {
                        EditorGUI.indentLevel++;

                        var consequentProps = new List<SerializedProperty>
                        {
                            otherData.FindPropertyRelative("hLabelColor"),
                            otherData.FindPropertyRelative("sLabelColor"),
                            otherData.FindPropertyRelative("vLabelColor")
                        };

                        DrawConsequentProperties(consequentProps);

                        EditorGUI.indentLevel--;
                    }

                    EditorGUI.indentLevel--;
                }

                guiEnabled = includeAlphaInput.boolValue || includeColorInput.boolValue;

                if (guiEnabled)
                {
                    UpdateVerticalPosition();

                    SerializedProperty includeInputFields = otherData.FindPropertyRelative("includeInputFields");
                    includeInputFields.boolValue = EditorGUI.ToggleLeft(positionRect, includeInputFields.displayName, includeInputFields.boolValue);

                    UpdateVerticalPosition();
                    EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("colorSliderTextureSize"));
                }

                EditorGUI.indentLevel--;
            }

            SerializedProperty includeUpperPanel = otherData.FindPropertyRelative("includeUpperPanel");

            UpdateVerticalPosition();
            guiEnabled = includeUpperPanel.boolValue = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include Upper Panel"), includeUpperPanel.boolValue);
            EditorGUI.Foldout(positionRect, guiEnabled, GUIContent.none, true);

            if (guiEnabled)
            {
                EditorGUI.indentLevel++;

                UpdateVerticalPosition();
                EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("upperPanelColor"));

                SerializedProperty includeMemorySwatches = otherData.FindPropertyRelative("includeMemorySwatches");
                SerializedProperty includeColorPicker = otherData.FindPropertyRelative("includeColorPicker");

                UpdateVerticalPosition();
                includeMemorySwatches.boolValue = EditorGUI.ToggleLeft(positionRect, includeMemorySwatches.displayName, includeMemorySwatches.boolValue);

                UpdateVerticalPosition();
                guiEnabled = includeColorPicker.boolValue = EditorGUI.ToggleLeft(positionRect, includeColorPicker.displayName, includeColorPicker.boolValue);

                if (guiEnabled)
                {
                    EditorGUI.indentLevel++;
                    UpdateVerticalPosition();
                    EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("pixelatedDisplayBackgroundColor"));
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }

            SerializedProperty includeUppermostPanel = otherData.FindPropertyRelative("includeTitlePanel");
            UpdateVerticalPosition();
            guiEnabled = includeUppermostPanel.boolValue = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include Title Panel"), includeUppermostPanel.boolValue);
            EditorGUI.Foldout(positionRect, guiEnabled, GUIContent.none);

            if (guiEnabled)
            {
                EditorGUI.indentLevel++;

                UpdateVerticalPosition();
                EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("titlePanelColor"));

                SerializedProperty includeTitle = otherData.FindPropertyRelative("includeTitle");
                SerializedProperty includeCloseButton = otherData.FindPropertyRelative("includeCloseButton");

                UpdateVerticalPosition();
                guiEnabled = includeTitle.boolValue = EditorGUI.ToggleLeft(positionRect, includeTitle.displayName, includeTitle.boolValue);

                if (guiEnabled)
                {
                    EditorGUI.indentLevel++;
                    UpdateVerticalPosition();
                    EditorGUI.PropertyField(positionRect, otherData.FindPropertyRelative("title"));
                    EditorGUI.indentLevel--;
                }

                UpdateVerticalPosition();
                includeCloseButton.boolValue = EditorGUI.ToggleLeft(positionRect, new GUIContent("Include Close Button"), includeCloseButton.boolValue);

                EditorGUI.indentLevel--;
            }
        }

        private int GetOtherSettingsHeight(SerializedProperty property)
        {
            SerializedProperty otherData = property.FindPropertyRelative("otherData");

            if (otherData is null)
                return 0;

            int height = 3;

            if (otherData.FindPropertyRelative("includeLowerPanel").boolValue)
            {
                height += 5;

                bool slidersAndInputFields = false;

                if (otherData.FindPropertyRelative("includeHexadecimalInput").boolValue)
                    height++;

                if (otherData.FindPropertyRelative("includeAlphaInput").boolValue)
                {
                    slidersAndInputFields = true;
                    height++;
                }

                if (otherData.FindPropertyRelative("includeColorInput").boolValue)
                {
                    slidersAndInputFields = true;
                    height += 3;

                    if (otherData.FindPropertyRelative("includeHSV").boolValue)
                        height += 3;

                    if (otherData.FindPropertyRelative("includeRGB0_255").boolValue || otherData.FindPropertyRelative("includeRGB0_1").boolValue)
                        height += 3;
                }

                if (slidersAndInputFields)
                    height += 2;
            }

            if (otherData.FindPropertyRelative("includeUpperPanel").boolValue)
            {
                height += 3;

                if (otherData.FindPropertyRelative("includeColorPicker").boolValue)
                    height++;
            }

            if (otherData.FindPropertyRelative("includeTitlePanel").boolValue)
            {
                height += 3;

                if (otherData.FindPropertyRelative("includeTitle").boolValue)
                    height++;
            }

            return height;
        }
    }
}