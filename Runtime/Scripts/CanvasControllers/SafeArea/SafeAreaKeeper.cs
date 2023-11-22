using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public interface SafeAreaKeeper
    {
        public void SetSafeArea(bool status);
        public bool IsSafeAreaSet();
    }
}