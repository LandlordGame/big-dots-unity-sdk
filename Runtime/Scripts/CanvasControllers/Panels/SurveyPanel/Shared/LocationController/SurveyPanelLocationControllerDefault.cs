using SurveyAPI.GPS;
using SurveyAPI.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SurveyAPI.CanvasControllers
{
    public class SurveyPanelLocationControllerDefault : SurveyPanelLocationControllerBase
    {
        [Header("External")]
        [SerializeField] NewLocationHandler newLocationHandler;
        [SerializeField] GPSServiceBase gps;

        [Header("Config")]
        [SerializeField] string newPositionDefaultLabel = "new place";

        private bool changingPosition;
        private PositionDouble placePosition;


        public override void Show(string poiName, double lat, double lon)
        {
            ResetState();

            SetupNewLocationHandler(poiName,lat,lon);
        }
        public override void Show()
        {
            Show("",gps.GetLastGPSPosition().Lat,gps.GetLastGPSPosition().Lon);
        }

        public override void Hide()
        {
            ResetState();
        }
        private void ResetState()
        {
            changingPosition = false;
            placePosition = null;

            newLocationHandler.ResetView();
        }

        public override void ToggleChangingPositionMode()
        {
            changingPosition = !changingPosition;
        }
        public override void UpdatePlacePositionLabel(string text)
        {
            if (text != null && text.Length > 0)
                newLocationHandler.SetNewPositionLabel(text); else
                newLocationHandler.SetNewPositionLabel(newPositionDefaultLabel);
        }
        
        private void SetupNewLocationHandler(string poiName, double lat, double lon)
        {
            newLocationHandler.ResetView();
            newLocationHandler.SetNewPositionLabel(newPositionDefaultLabel);

            Action<double,double> newLocationAction = (double lat,double lon) =>
            {
                placePosition = new PositionDouble(lat,lon);
            };

            newLocationHandler.HandleNewPOI(poiName,lat,lon,newLocationAction);
        }
        
        public override void HandleMapPointerDownEvent(PointerEventData eventData)
        {
            if (changingPosition == true)
                newLocationHandler.HandleMapPointerDownEvent(eventData);
        }

        public override bool GetChangingPositionMode()
        {
            return changingPosition;
        }
        public override PositionDouble GetPlacePosition()
        {
            return placePosition;
        }
        public override PositionDouble GetLastUserPosition()
        {
            if (gps.GetLastGPSPosition() == null)
                return null;

            return new PositionDouble(gps.GetLastGPSPosition().Lat,gps.GetLastGPSPosition().Lon);
        }
    }
}