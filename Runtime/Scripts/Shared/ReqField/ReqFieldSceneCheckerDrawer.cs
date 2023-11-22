#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SurveyAPI.Shared
{
    [CustomEditor(typeof(ReqFieldSceneChecker))]
    public class ReqFieldSceneCheckerDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            ReqFieldSceneChecker unityObject = (ReqFieldSceneChecker)target;

            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();
       
            if (GUILayout.Button("Check fields now!"))
                unityObject.CheckRequiredFields();

            EditorGUILayout.EndVertical();
        }
    }
}
#endif