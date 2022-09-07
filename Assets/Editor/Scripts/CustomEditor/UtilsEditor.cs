using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UtilityEditor
{
    [CustomEditor(typeof(MonoBehaviour))]
    public class UtilsEditor : Editor
    {
        /*public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.
        }*/
        public class ReadOnlyAttribute : PropertyAttribute
        {

        }

        [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
        public class ReadOnlyDrawer : PropertyDrawer
        {
            /// <summary>
            /// Unity method for drawing GUI in Editor
            /// </summary>
            /// <param name="position">Position.</param>
            /// <param name="property">Property.</param>
            /// <param name="label">Label.</param>
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                // Saving previous GUI enabled value
                var previousGUIState = GUI.enabled;
                // Disabling edit for property
                GUI.enabled = false;
                // Drawing Property
                EditorGUI.PropertyField(position, property, label);
                // Setting old GUI enabled value
                GUI.enabled = previousGUIState;
            }
        }

        public class TextPopup : EditorWindow
        {
            Action<string> _callback;
            string _text;

            string inputText = "";

            private void OnGUI()
            {
            
                GUILayout.Space(10);
                EditorGUILayout.LabelField(_text, EditorStyles.wordWrappedLabel);
                GUILayout.Space(10);
                //Debug.Log(EditorGUILayout.TextField("", inputText));
                inputText = EditorGUILayout.TextField("", inputText);
                GUILayout.Space(60);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save"))
                {
                    //Debug.Log(inputText);
                    _callback(inputText);
                    Close();
                }

                if (GUILayout.Button("Cancel")) Close();
                EditorGUILayout.EndHorizontal();
            }

            public TextPopup(Action<string> method, string text = "Text here:")
            {
                _callback = method;
                _text = text;
            }
        }
    }
}

