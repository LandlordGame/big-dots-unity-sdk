#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SurveyAPI.GPS
{
    [CustomPropertyDrawer(typeof(FakeGPSPosition))]
    public class FakeGPSPositionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var lat = property.FindPropertyRelative("lat");
            var lon = property.FindPropertyRelative("lon");

            EditorGUI.LabelField(new Rect(position.x, position.y, 100, position.height),label);
            lat.doubleValue = EditorGUI.DoubleField(new Rect(position.x + 100, position.y, 100, position.height),lat.doubleValue);
            lon.doubleValue = EditorGUI.DoubleField(new Rect(position.x + 200, position.y, 100, position.height),lon.doubleValue);

            EditorGUI.EndProperty();
        }
    }
}

#endif