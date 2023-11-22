using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public interface GPSListener
    {
        public void HandleGPSData(GPSData data);
    }
}