using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SurveyAPI.GPS
{
    public class GPSDataProviderFake : GPSDataProviderBase
    {
        [SerializeField] private float timeToInit = 5;
        [SerializeField] private GPSStatus statusBeforeInit = GPSStatus.STOPPED;
        [SerializeField] private GPSStatus statusAfterInit = GPSStatus.RUNNING;

        [SerializeField] private FakeGPSPosition[] fakeRoute;
        [SerializeField] private bool loopRoute = false;
        [SerializeField] private bool stopAfterLastPosition = true;
        [SerializeField] private bool autoRouteChange = true;
        [SerializeField] private float routeChangeIntervalInSeconds = 5;

        private int currentPosition = 0;
        private GPSStatus currentStatus = GPSStatus.STOPPED;
        private float timeToInitCounter;
        private float routeChangeCounter;


        private void Awake()
        {
            currentStatus = statusBeforeInit;
        }
        private void Update()
        {
            ProcessInitialStatusChange();
            ProcessRouteChange();
        }
        private void ProcessInitialStatusChange()
        {
            if (timeToInitCounter < timeToInit)
            {
                timeToInitCounter += Time.deltaTime;
                if (timeToInitCounter >= timeToInit)
                    currentStatus = statusAfterInit;
            }
        }
        private void ProcessRouteChange()
        {
            if (IsGPSWorking() == false || IsAutoRouteEnabled() == false)
                return;

            routeChangeCounter += Time.deltaTime;
            if (routeChangeCounter >= routeChangeIntervalInSeconds)
            {
                MoveToNextPosition();

                routeChangeCounter -= routeChangeIntervalInSeconds;
            }
        }
        public bool IsGPSWorking()
        {
            return currentStatus == GPSStatus.RUNNING;
        }
        public bool IsAutoRouteEnabled()
        {
            if (autoRouteChange == false || routeChangeIntervalInSeconds <= 0)
                return false; else
                return true;
        }
        public void SetAutoRouteStatus(bool status)
        {
            autoRouteChange = status;
        }

        public override GPSData GetLastPosition()
        {
            if (IsGPSWorking() == true)
                return new GPSData(0,fakeRoute[currentPosition].lat,fakeRoute[currentPosition].lon); else
                return null;
        }
        public override GPSStatus GetStatus()
        {
            return currentStatus;
        }

        public void MoveToNextPosition()
        {
            currentPosition++;
            if (currentPosition >= fakeRoute.Length)
            {
                if (stopAfterLastPosition == true)
                {
                    //currentStatus = GPSStatus.STOPPED;
                    currentPosition = fakeRoute.Length-1;
                } else
                {
                    if (loopRoute == true)
                        currentPosition = 0; else
                        currentPosition = fakeRoute.Length-1;
                }
            }
        }
    }
}