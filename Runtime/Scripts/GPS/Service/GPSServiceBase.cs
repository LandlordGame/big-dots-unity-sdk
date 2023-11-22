using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public abstract class GPSServiceBase : MonoBehaviour, GPSService
    {
        public abstract void AddListener(GPSListener listener);
        public abstract void RemoveListener(GPSListener listener);

        public abstract GPSStatus GetState();
        public abstract GPSData GetLastGPSPosition();
        public abstract GPSData[] GetGPSHistory(int count);
    }
}
