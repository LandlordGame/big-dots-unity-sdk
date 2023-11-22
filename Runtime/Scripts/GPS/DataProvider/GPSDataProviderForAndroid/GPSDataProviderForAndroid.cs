using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Android;


namespace SurveyAPI.GPS
{
    public class GPSDataProviderForAndroid : GPSDataProviderBase
    {
        private bool permissionGranted = false;

        void Start()
        {
            AskForPermission();
        }
        private void AskForPermission()
        {
            PermissionCallbacks callbacks = new PermissionCallbacks();
            callbacks.PermissionDenied += (s) =>
            {
                
            };
            callbacks.PermissionDeniedAndDontAskAgain += (s) =>
            {

            };
            callbacks.PermissionGranted += (s) =>
            {
                Input.location.Start();
                
                permissionGranted = true;
            };
 
            Permission.RequestUserPermission(Permission.FineLocation,callbacks);
        }


        public override GPSData GetLastPosition()
        {
            LocationInfo info = Input.location.lastData;

            return new GPSData((long) info.timestamp,info.latitude,info.longitude);
        }
        public override GPSStatus GetStatus()
        {
            if (permissionGranted == true)
                return (GPSStatus)Input.location.status; else
                return GPSStatus.FAILED;
        }
    }
}