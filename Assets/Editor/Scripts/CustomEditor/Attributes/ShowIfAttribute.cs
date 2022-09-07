using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UtilityEditor
{
	public class ShowIfAttribute : PropertyAttribute
	{
		public string conditionPropertyName { get; private set; }
		public bool readonlyInstead { get; private set; }

		public ShowIfAttribute(string conditionPropertyName, bool readonlyInstead = false)
		{
			this.conditionPropertyName = conditionPropertyName;
			this.readonlyInstead = readonlyInstead;
		}
	}

	[CustomPropertyDrawer(typeof(ShowIfAttribute))]

	public class ShowIfDrawer : PropertyDrawer
	{
		ShowIfAttribute showIf;

		/// <summary>
		/// Unity method for drawing GUI in Editor
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="property">Property.</param>
		/// <param name="label">Label.</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			showIf = attribute as ShowIfAttribute;

			//Debug.Log(showIf.conditionPropertyName);

			SerializedProperty comparedField = property.serializedObject.FindProperty(showIf.conditionPropertyName);

			// Get the value of the compared field.
			bool value = comparedField.boolValue;

			if(showIf.readonlyInstead)
            {
				// Saving previous GUI enabled value
				var previousGUIState = GUI.enabled;
				// Disabling edit for property
				GUI.enabled = value;
				// Drawing Property
				EditorGUI.PropertyField(position, property, label);
				// Setting old GUI enabled value
				GUI.enabled = previousGUIState;
			}
			else if(value)
            {
				//a = property.

				// Saving previous GUI enabled value
				var previousGUIState = GUI.enabled;
				// Disabling edit for property
				GUI.enabled = true;
				// Drawing Property
				EditorGUI.PropertyField(position, property, label);
				// Setting old GUI enabled value
				GUI.enabled = previousGUIState;
            }

		}
	}

	public class HideIfAttribute : PropertyAttribute
	{
		public string conditionPropertyName { get; private set; }
		public bool readonlyInstead { get; private set; }

		/// <summary>
		/// Only draws the field only if a condition is met.
		/// </summary>
		/// <param name="comparedPropertyName">The name of the property that is being compared (case sensitive).</param>
		/// <param name="comparedValue">The value the property is being compared to.</param>
		/// <param name="comparisonType">The type of comparison the values will be compared by.</param>
		/// <param name="disablingType">The type of disabling that should happen if the condition is NOT met. Defaulted to DisablingType.DontDraw.</param>
		public HideIfAttribute(string conditionPropertyName, bool readonlyInstead = false)
		{
			this.conditionPropertyName = conditionPropertyName;
			this.readonlyInstead = readonlyInstead;
		}
	}

	[CustomPropertyDrawer(typeof(HideIfAttribute))]

	public class HideIfDrawer : PropertyDrawer
	{
		HideIfAttribute showIf;

		/// <summary>
		/// Unity method for drawing GUI in Editor
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="property">Property.</param>
		/// <param name="label">Label.</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			showIf = attribute as HideIfAttribute;

			//Debug.Log(showIf.conditionPropertyName);

			SerializedProperty comparedField = property.serializedObject.FindProperty(showIf.conditionPropertyName);

			// Get the value of the compared field.
			bool value = !comparedField.boolValue;

			if (showIf.readonlyInstead)
			{
				// Saving previous GUI enabled value
				var previousGUIState = GUI.enabled;
				// Disabling edit for property
				GUI.enabled = value;
				// Drawing Property
				EditorGUI.PropertyField(position, property, label);
				// Setting old GUI enabled value
				GUI.enabled = previousGUIState;
			}
			else if (value)
			{
				//a = property.

				// Saving previous GUI enabled value
				var previousGUIState = GUI.enabled;
				// Disabling edit for property
				GUI.enabled = true;
				// Drawing Property
				EditorGUI.PropertyField(position, property, label);
				// Setting old GUI enabled value
				GUI.enabled = previousGUIState;
			}

		}
	}
}
