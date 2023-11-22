using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public interface GPSDataProvider
    {
        public GPSData GetLastPosition();
        public GPSStatus GetStatus();
    }
}
