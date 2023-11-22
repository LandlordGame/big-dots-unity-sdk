using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public interface GPSService
    {
        public void AddListener(GPSListener listener);
        public void RemoveListener(GPSListener listener);

        public GPSStatus GetState();
        public GPSData GetLastGPSPosition();
        public GPSData[] GetGPSHistory(int count);
    }
}