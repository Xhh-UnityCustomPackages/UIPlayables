using System;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using U = UnityEngine.Rendering.ShaderPropertyType;

namespace TweenPlayables.Editor
{
    [CustomPropertyDrawer(typeof(TweenMaterialBehaviour))]
    public class TweenMaterialBehaviourDrawer : PropertyDrawer
    {
        protected GUIContent ValueLabel => new GUIContent("Value");

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw dropdown to choose a shader property to manipulate
            DrawPropertyDropdown(position, property);
            SerializedProperty nameProp = property.FindPropertyRelative("propertyName");

            // Draw UI to manipulate the chosen shader property
            if (!string.IsNullOrEmpty(nameProp.stringValue))
            {
                DrawValueProperty(property);
                RefreshObject(property.serializedObject);
            }
        }

        void DrawPropertyDropdown(Rect position, SerializedProperty root)
        {
            SerializedProperty nameProp = root.FindPropertyRelative("propertyName");
            Rect dropdownLabel = EditorGUI.PrefixLabel(new Rect(position.x,
                    position.y,
                    position.width,
                    EditorGUIUtility.singleLineHeight),
                new GUIContent("Property"));
            bool dropdownOpen = EditorGUI.DropdownButton(dropdownLabel,
                new GUIContent(nameProp.stringValue),
                FocusType.Keyboard);

            if (dropdownOpen)
                DrawMaterialPropertyList(dropdownLabel, root);
        }

        void DrawMaterialPropertyList(Rect position, SerializedProperty root)
        {
            var target = GetTarget(root);

            var material = GetAffectedMaterials(target);

            if (material == null) return;

            var propertyNames = new HashSet<string>();
            var props = MaterialEditor.GetMaterialProperties(new Material[] { material });

            foreach (MaterialProperty p in props)
                propertyNames.Add(p.name);

            Action<string> OnSelectionChanged = entry =>
            {
                // Choose the first material that has the selected property
                // to retrieve the corresponding shader property type

                Shader shader = material.shader;
                int propIndex = shader.FindPropertyIndex(entry);
                if (propIndex < 0)
                        // Shader doesn't have any property with selected name
                    return;

                target.propertyName = entry;
                target.propertyType = shader.GetPropertyType(propIndex);
                target.ApplyFromMaterial(material);

                // Ensure selected entry is triggering updates immediately
                RefreshObject(root.serializedObject);
                TimelineEditor.Refresh(RefreshReason.ContentsModified);
            };

            var treeView = new StringTreeView(propertyNames, OnSelectionChanged);
            var treeViewPopup = new TreeViewPopupWindow(treeView)
            {
                Width = position.width
            };
            PopupWindow.Show(position, treeViewPopup);
        }

        void DrawValueProperty(SerializedProperty root)
        {
            SerializedProperty typeP = root.FindPropertyRelative("propertyType");
            SerializedProperty vecP = root.FindPropertyRelative("vector");
            
            bool hasMultipleValues = vecP.hasMultipleDifferentValues || typeP.hasMultipleDifferentValues;
            
            EditorGUI.showMixedValue = hasMultipleValues;
            if (EditorGUI.showMixedValue)
            {
                EditorGUI.showMixedValue = false;
                return;
            }

            Vector4 vecVal = vecP.vector4Value;
            switch ((U)typeP.enumValueIndex)
            {
                case U.Texture:
                    Rect rect = EditorGUILayout.GetControlRect(
                        hasLabel: true,
                        height: EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing);
                    vecP.vector4Value = MaterialEditor.TextureScaleOffsetProperty(rect, vecP.vector4Value);
                    break;
                case U.Color:
                    vecP.vector4Value = EditorGUILayout.ColorField(
                        label: ValueLabel,
                        value: vecVal,
                        showEyedropper: true,
                        showAlpha: true,
                        hdr: true);
                    break;
                case U.Vector:
                    EditorGUILayout.PropertyField(vecP, ValueLabel);
                    break;
                case U.Float:
                    vecVal.x = EditorGUILayout.FloatField(ValueLabel, vecVal.x);
                    vecP.vector4Value = vecVal;
                    break;
                case U.Range:
                    vecVal.x = EditorGUILayout.Slider(
                        label: ValueLabel,
                        value: vecVal.x,
                        leftValue: vecVal.y,
                        rightValue: vecVal.z);
                    vecP.vector4Value = vecVal;
                    break;
            }
            
            EditorGUI.showMixedValue = false;
        }

        void RefreshObject(SerializedObject toRefresh)
        {
            toRefresh.ApplyModifiedProperties();
            toRefresh.Update();
        }

        TweenMaterialBehaviour GetTarget(SerializedProperty root)
        {
            var targetObject = root.serializedObject.targetObject;
            return fieldInfo.GetValue(targetObject) as TweenMaterialBehaviour;
        }

        Material GetAffectedMaterials(TweenMaterialBehaviour target)
        {
            if (target != null && target.mixer != null && target.mixer.material != null)
                return target.mixer.material;

            // Ensure that the timeline rebuilds the graph, so that
            // the material provider could initialize
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
            return null;
        }
    }
}