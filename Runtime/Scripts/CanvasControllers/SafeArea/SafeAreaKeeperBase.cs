using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public abstract class SafeAreaKeeperBase : MonoBehaviour, SafeAreaKeeper
    {
        public abstract void SetSafeArea(bool status);
        public abstract bool IsSafeAreaSet();
    }
}
