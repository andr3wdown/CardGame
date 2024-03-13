using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{
	private CardData cardData;
	private string[] properties = {"cardName", "cost", "HP", "ATK", "energy", "shards", "speed", "effectText", "cardImage", "imagePositioning", "imageScaling" };

	public override void OnInspectorGUI()
	{
		cardData = (CardData)target;
		SerializedObject serializedObject = new SerializedObject(cardData);
		serializedObject.Update();

		for (int i = 0; i < properties.Length; i++)
		{
			SerializedProperty serializedProperty = serializedObject.FindProperty(properties[i]);
			if (properties[i] == "effectText")
			{
				DrawLargeTextField(serializedProperty);
			}
			else
			{
				Draw(serializedProperty);
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
	private void Draw(SerializedProperty property)
	{
		EditorGUILayout.PropertyField(property);
	}
	private void DrawLargeTextField(SerializedProperty property)
	{
		EditorGUILayout.PropertyField(property, GUILayout.Height(100));
	}
}
