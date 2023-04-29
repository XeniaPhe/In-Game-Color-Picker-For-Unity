using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Xenia.ColorPicker.Core;
using Xenia.ColorPicker.EditorScripts.Serialization;

namespace Xenia.ColorPicker.EditorScripts
{
    [CustomEditor(typeof(ColorPickerBuilder))]
    internal class ColorPickerBuilderEditor : Editor
    {
        private ColorPickerBuilder builder;

        private readonly string[] invalidCharacters = { "/", "?", "<", ">", "\\", ":", "*", "|", "\"" };

        private string configsPath;
        private string configsPathShort;

        private List<ColorPickerConfiguration> configs;
        private string[] configsNames;
        private int selectedIndex = 0;
        private string saveName = string.Empty;

        private static bool showReferences = true;
        private static bool editCustomConfigs = true;
        private static bool showCustomConfigs = true;
        private static bool showFromSave = false;

        private void OnEnable()
        {
            string assetsDir = Application.dataPath;

            string[] temp;

            temp = Directory.GetDirectories(assetsDir, "InGameColorPicker", SearchOption.TopDirectoryOnly);

            if (temp.Length == 0)
                temp = Directory.GetDirectories(assetsDir, "InGameColorPicker", SearchOption.AllDirectories);

            configsPath = (temp.Length == 1 ? temp[0] : assetsDir) + "/Configurations/";
            configsPathShort = configsPath.Substring(configsPath.IndexOf("Assets"));

            builder = (ColorPickerBuilder)target;
            saveName = string.Empty;
            configs = new List<ColorPickerConfiguration>();
            configsNames = new string[0];
            LoadConfigs();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawCustomConfigs();
            DrawSavedConfigs();
            DrawButtons();
            DrawReferences();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawReferences()
        {
            EditorGUILayout.Space();

            showReferences = EditorGUILayout.BeginFoldoutHeaderGroup(showReferences,
                new GUIContent("References", "Something might have gone null here if the builder isn't working"));

            if (showReferences)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(serializedObject.FindProperty("hueDrawer"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lowerPanel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rhRow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gsRow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bvRow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("aRow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("hexadecimalRow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("swatchesRow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("upperPanel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pixelatedDisplay"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("memorySwatches"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("titlePanel"));

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawCustomConfigs()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            showCustomConfigs = EditorGUILayout.BeginFoldoutHeaderGroup(showCustomConfigs, "Custom Configuration");
            editCustomConfigs = EditorGUILayout.Toggle(string.Empty, editCustomConfigs);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndHorizontal();

            if (showCustomConfigs)
            {
                EditorGUI.BeginDisabledGroup(!editCustomConfigs);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reversableData"));
                EditorGUI.EndDisabledGroup();
            }
        }

        private void DrawSavedConfigs()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            showFromSave = EditorGUILayout.BeginFoldoutHeaderGroup(showFromSave, "Saved Configuration");
            editCustomConfigs = !EditorGUILayout.Toggle(string.Empty, !editCustomConfigs);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndHorizontal();

            if (showFromSave)
            {
                EditorGUI.BeginDisabledGroup(editCustomConfigs);

                selectedIndex = EditorGUILayout.Popup("Select", selectedIndex, configsNames);

                EditorGUILayout.Space();

                DrawSave();

                if (configs is not null && configs.Count != 0)
                {
                    if (GUILayout.Button("Delete"))
                    {
                        string name = configsNames[selectedIndex];
                        ArrayUtility.RemoveAt(ref configsNames, selectedIndex);
                        configs.RemoveAt(selectedIndex);

                        AssetDatabase.DeleteAsset(GetFullConfigPath(name));
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        LoadConfigs();
                    }
                }

                EditorGUI.EndDisabledGroup();
            }
        }

        private void DrawButtons()
        {
            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(!IsUpdated());

            if (GUILayout.Button("Build"))
            {
                if (!editCustomConfigs)
                {
                    var selectedConfig = configs[selectedIndex];

                    Texture2D hueDrawerTex = new Texture2D(1, 1);
                    Texture2D hueSelectorTex = new Texture2D(1, 1);
                    Texture2D svSelectorTex = new Texture2D(1, 1);

                    hueDrawerTex.LoadImage(selectedConfig.hueDrawerPNG);
                    hueSelectorTex.LoadImage(selectedConfig.hueSelectorPNG);
                    svSelectorTex.LoadImage(selectedConfig.svSelectorPNG);

                    builder.ApplySavedSettings(selectedConfig.data, hueDrawerTex, hueSelectorTex, svSelectorTex);
                }
                else
                {
                    builder.ApplyChangesAndBuild();
                }

                SceneView.RepaintAll();
                GUI.FocusControl(null);
            }

            if (GUILayout.Button("Revert"))
            {
                Revert();
                GUI.FocusControl(null);
            }

            EditorGUI.EndDisabledGroup();


            if (GUILayout.Button("Default"))
            {
                builder.BuildDefault();
                SceneView.RepaintAll();
            }
        }

        private void DrawSave()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel("Save As");

            saveName = EditorGUILayout.TextField(saveName);

            EditorGUILayout.EndHorizontal();

            foreach (var ch in invalidCharacters)
            {
                if (saveName.Contains(ch))
                {
                    saveName = saveName.Replace(ch, string.Empty);
                    GUI.FocusControl(null);
                }
            }

            bool saveButtonDisabled = true;

            if (!saveName.Equals(string.Empty))
            {
                saveButtonDisabled = false;

                foreach (var name in configsNames)
                {
                    if (name.Equals(saveName))
                    {
                        saveButtonDisabled = true;
                        break;
                    }
                }
            }

            EditorGUI.BeginDisabledGroup(saveButtonDisabled);

            if (GUILayout.Button("Save Current Build"))
            {
                var save = ScriptableObject.CreateInstance<ColorPickerConfiguration>();

                var sprites = builder.GetTexturesDrawn();

                save.name = saveName;
                save.hueDrawerPNG = sprites.hueDrawerTexture.EncodeToPNG();
                save.hueSelectorPNG = sprites.hueSelectorTexture.EncodeToPNG();
                save.svSelectorPNG = sprites.svSelectorTexture.EncodeToPNG();
                save.data = builder.CurrentData;

                if (!Directory.Exists(configsPath))
                    Directory.CreateDirectory(configsPath);

                AssetDatabase.CreateAsset(save, GetFullConfigPath(saveName));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                configs.Add(save);
                ArrayUtility.Add(ref configsNames, saveName);

                saveName = string.Empty;
                GUI.FocusControl(null);
            }

            EditorGUI.EndDisabledGroup();
        }

        private void LoadConfigs()
        {
            if (!Directory.Exists(configsPath))
                return;

            string[] guids = AssetDatabase.FindAssets
                ("t:Xenia.ColorPicker.EditorScripts.Serialization.ColorPickerConfiguration", new string[] { configsPathShort });

            IEnumerable<string> paths = guids.Select(g => AssetDatabase.GUIDToAssetPath(g));
            configs = paths.Select(p => AssetDatabase.LoadAssetAtPath<ColorPickerConfiguration>(p)).ToList();
            configsNames = configs.Select(c => c.name).ToArray();
        }

        private string GetFullConfigPath(string configName)
        {
            return string.Concat(configsPathShort, configName, ".asset");
        }

        private bool IsUpdated()
        {
            bool updated;

            if (configs.Count > 0 && !editCustomConfigs)
                updated = !builder.CurrentData.Equals(configs[selectedIndex].data);
            else
                updated = builder.Updated && editCustomConfigs;

            return updated;
        }

        private void Revert()
        {
            builder.Revert();
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