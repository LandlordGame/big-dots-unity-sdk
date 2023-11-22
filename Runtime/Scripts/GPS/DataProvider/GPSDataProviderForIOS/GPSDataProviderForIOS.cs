using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public class GPSDataProviderForIOS : GPSDataProviderBase
    {
        void Start()
        {
            Input.location.Start();
        }

        public override GPSData GetLastPosition()
        {
            LocationInfo info = Input.location.lastData;

            return new GPSData((long) info.timestamp,info.latitude,info.longitude);
        }
        public override GPSStatus GetStatus()
        {
            return (GPSStatus)Input.location.status;
        }
    }
}