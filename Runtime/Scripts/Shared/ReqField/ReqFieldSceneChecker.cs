#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;

namespace SurveyAPI.Shared
{
    [ExecuteInEditMode]
    public class ReqFieldSceneChecker : MonoBehaviour
    {
        [Header("Autorun Config")]
        [SerializeField] bool autorunBeforeEnteringPlayMode = false;
        [SerializeField] bool enterPlayModeWithWarnings = false;

        [Header("Fields to check")]
        [SerializeField] bool checkAllFields = false;
        [SerializeField] bool checkFieldsWithSerializeField = false;
        [SerializeField] bool checkFieldsWithReqField = true;

        private Action sceneChangeAction;
        private Action<PlayModeStateChange> enterPlayModeAction;


        private void OnEnable()
        {
            //AddSceneChangeListener();
        
            if (autorunBeforeEnteringPlayMode == true)
                AddEnterPlayModeListener();
        }
        private void OnDisable()
        {
            //RemoveSceneChangeListener();
        
            if (autorunBeforeEnteringPlayMode == true)
                RemoveEnterPlayModeListener();
        }

        private void AddSceneChangeListener()
        {
            sceneChangeAction = () => { CheckRequiredFields(); };
            EditorApplication.hierarchyChanged += sceneChangeAction;
        }
        private void RemoveSceneChangeListener()
        {
            EditorApplication.hierarchyChanged -= sceneChangeAction;
        }

        private void AddEnterPlayModeListener()
        {
            enterPlayModeAction = (PlayModeStateChange stateChanged) =>
            {
                if (stateChanged != PlayModeStateChange.ExitingEditMode)
                    return;

                bool allFieldsOK = CheckRequiredFields();
                if (allFieldsOK == false && enterPlayModeWithWarnings == false)
                    EditorApplication.ExitPlaymode();
            };

            EditorApplication.playModeStateChanged += enterPlayModeAction;
        }
        private void RemoveEnterPlayModeListener()
        {
            EditorApplication.playModeStateChanged -= enterPlayModeAction;
        }

        public bool CheckRequiredFields()
        {
            Debug.Log("ReqFieldSceneChecker - checking required fields...");
        
            bool result = true;
            int nullReferenceCount = 0;
            MonoBehaviour[] scripts = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                FieldInfo[] fields = script.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo field in fields)
                {
                    if (checkAllFields == true ||
                       (checkFieldsWithSerializeField == true && field.GetCustomAttributes(typeof(SerializeField),true).Length > 0) ||
                       (checkFieldsWithReqField == true && field.GetCustomAttributes(typeof(ReqField),true).Length > 0))
                    {
                        object value = field.GetValue(script);
                        if (value == null)
                        {
                            Debug.LogError($"Found null reference field in GameObject: {script.name} ({field.Name})");
                            nullReferenceCount++;
                            result = false;
                        }   
                    }
                }
            }

            if (result == true)
                Debug.Log("ReqFieldSceneChecker - all inspected fields are set [OK]"); else
                Debug.LogError($"ReqFieldSceneChecker - {nullReferenceCount} of inspected fields are NOT set [FAIL]");

            return result;
        }
    }
}
#endif