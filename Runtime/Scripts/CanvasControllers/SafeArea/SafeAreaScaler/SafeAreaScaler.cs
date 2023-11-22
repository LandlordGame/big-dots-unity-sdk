using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class SafeAreaScaler : SafeAreaKeeperBase
    {
        [System.Serializable]
        public class MyRect
        {
            public float leftPadding;
            public float rightPadding;
            public float downPadding;
            public float topPadding;

            public MyRect()
            {
                
            }
            public MyRect(float leftPadding, float rightPadding, float downPadding, float topPadding)
            {
                this.leftPadding = leftPadding;
                this.rightPadding = rightPadding;
                this.downPadding = downPadding;
                this.topPadding = topPadding;
            }
        }
        [System.Serializable]
        public class MyContainer
        {
            public RectTransform container;
            public CanvasScaler containerCanvasScaler;

            public bool scaleHorizontal;
            public bool scaleVertical;
            public bool keepAspectRatio;
        }

        [SerializeField] private bool enableSafeAreaOnAwake = false;
        [SerializeField] private bool useTestPadding = false;
        [SerializeField] private MyRect testPadding; // iPhoneX: l:132,p:132,d:63,g:0
        [SerializeField] private MyContainer[] safeAreaContainers;

        private bool safeAreaSet = false;

        
        void Awake()
        {
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

        public void DisableSafeArea()
        {
            safeAreaSet = false;

            foreach (MyContainer safeAreaContainer in this.safeAreaContainers)
            {
                safeAreaContainer.container.offsetMin = Vector2.zero;
                safeAreaContainer.container.offsetMax = Vector2.zero;

                safeAreaContainer.container.localScale = Vector2.one;
            }
        }
        public void EnableSafeArea()
        {
            safeAreaSet = true;

            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
            Rect safeRect;
            MyRect padding;

            if (useTestPadding == true)
            {
                safeRect = new Rect(
                    testPadding.leftPadding,
                    testPadding.downPadding,
                    screenRect.width - testPadding.leftPadding - testPadding.rightPadding,
                    screenRect.height - testPadding.downPadding - testPadding.topPadding);

                padding = testPadding;
            } else
            {
                safeRect = Screen.safeArea;

                padding = new MyRect(safeRect.x,screenRect.width - safeRect.xMax,
                    safeRect.y, screenRect.height - safeRect.yMax);
            }

            Debug.Log("SafeAreaScaler - screenRect: " + screenRect);
            Debug.Log("SafeAreaScaler - safeRect: " + safeRect);
            
            Debug.Log("SafeAreaScaler - leftPadding: " + padding.leftPadding);
            Debug.Log("SafeAreaScaler - rightPadding: " + padding.rightPadding);
            Debug.Log("SafeAreaScaler - downPadding: " + padding.downPadding);
            Debug.Log("SafeAreaScaler - topPadding: " + padding.topPadding);
            
            //float horizontalRatio = 1280 / screenRect.width;
            //float verticalRatio = 720 / screenRect.height;
            foreach (MyContainer safeAreaContainer in this.safeAreaContainers)
            {
                float horizontalRatio = safeAreaContainer.containerCanvasScaler.referenceResolution.y / screenRect.width;
                float verticalRatio = safeAreaContainer.containerCanvasScaler.referenceResolution.x / screenRect.height;

                safeAreaContainer.container.offsetMin = new Vector2(
                    padding.leftPadding * horizontalRatio,
                    padding.downPadding * verticalRatio);

                safeAreaContainer.container.offsetMax = new Vector2(
                    -padding.rightPadding * horizontalRatio,
                    -padding.topPadding * verticalRatio);

                Vector2 scale = Vector2.zero;
                scale.x = safeRect.width / screenRect.width;
                scale.y = safeRect.height / screenRect.height;

                if (safeAreaContainer.keepAspectRatio == true &&
                    safeAreaContainer.scaleHorizontal == true &&
                    safeAreaContainer.scaleVertical == true)
                {
                    if (scale.x < scale.y)
                        scale.y = scale.x; else
                        scale.x = scale.y;
                }
                if (safeAreaContainer.scaleHorizontal == false)
                    scale.x = 1;
                if (safeAreaContainer.scaleVertical == false)
                    scale.y = 1;

                safeAreaContainer.container.localScale = new Vector2(scale.x, scale.y);
            }
        }
    }
}
