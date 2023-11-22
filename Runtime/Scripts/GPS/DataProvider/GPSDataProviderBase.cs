using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public abstract class GPSDataProviderBase : MonoBehaviour, GPSDataProvider
    {
        public abstract GPSData GetLastPosition();
        public abstract GPSStatus GetStatus();
    }
}
