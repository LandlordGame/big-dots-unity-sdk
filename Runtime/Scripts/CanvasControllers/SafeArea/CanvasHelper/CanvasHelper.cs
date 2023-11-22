using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
namespace SurveyAPI.CanvasControllers
{
    public class CanvasHelper : SafeAreaKeeperBase
    {
        [System.Serializable]
        public class SafeAreaContainer
        {
            public Canvas canvas;
            public RectTransform safeAreaTransform;

            public Vector2 AnchorMin { get; set; }
            public Vector2 AnchorMax { get; set; }
        }

        [SerializeField] private bool enableSafeAreaOnAwake = false;
        [SerializeField] private SafeAreaContainer[] safeAreaContainers;
    
        private bool safeAreaSet = false;
 

 
        void Awake()
        {
            foreach (SafeAreaContainer safeContainer in safeAreaContainers)
            {
                safeContainer.AnchorMin = safeContainer.safeAreaTransform.anchorMin;
                safeContainer.AnchorMax = safeContainer.safeAreaTransform.anchorMax;
            }

            if (enableSafeAreaOnAwake == true)
                EnableSafeArea();
        }

        public override void SetSafeArea(bool status)
        {
            if (status == true)
                EnableSafeArea(); else
                DisableSafeArea();
        }
        public override bool IsSafeAreaSet()
        {
            return safeAreaSet;
        }

        private void EnableSafeArea()
        {
            safeAreaSet = true;

            foreach (SafeAreaContainer safeContainer in safeAreaContainers)
            {
                Canvas canvas = safeContainer.canvas;
                RectTransform safeRect = safeContainer.safeAreaTransform;

                var anchorMin = Screen.safeArea.min;
                var anchorMax = Screen.safeArea.max;
                anchorMin.x /= canvas.pixelRect.width;
                anchorMin.y /= canvas.pixelRect.height;
                anchorMax.x /= canvas.pixelRect.width;
                anchorMax.y /= canvas.pixelRect.height;
 
                safeRect.anchorMin = anchorMin;
                safeRect.anchorMax = anchorMax;
            }
        }
        private void DisableSafeArea()
        {
            safeAreaSet = false;

            foreach (SafeAreaContainer safeContainer in safeAreaContainers)
            {
                RectTransform safeRect = safeContainer.safeAreaTransform;
                safeRect.anchorMin = safeContainer.AnchorMin;
                safeRect.anchorMax = safeContainer.AnchorMax;
            }
        }
    }
}
