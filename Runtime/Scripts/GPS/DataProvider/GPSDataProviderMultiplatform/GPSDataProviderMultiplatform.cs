using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public class GPSDataProviderMultiplatform : GPSDataProviderBase
    {
        [SerializeField] private GPSDataProviderBase gpsForPC;
        [SerializeField] private GPSDataProviderBase gpsForAndroid;
        [SerializeField] private GPSDataProviderBase gpsForIOS;

        private GPSDataProviderBase currentProvider;

        private void Awake()
        {
            currentProvider = gpsForPC;

            if (Application.platform == RuntimePlatform.Android)
                currentProvider = gpsForAndroid; else
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                currentProvider = gpsForIOS;
        }

        public override GPSData GetLastPosition()
        {
            GPSData data = currentProvider.GetLastPosition();

            return data;
        }
        public override GPSStatus GetStatus()
        {
            return currentProvider.GetStatus();
        }
    }
}
