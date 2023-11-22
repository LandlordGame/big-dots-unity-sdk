using Mapbox.Utils;
using SurveyAPI.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class NewLocationHandler : MonoBehaviour
    {
        [Header("External")]
        [SerializeField] private Camera mapCamera;
        [SerializeField] private RawImage mapImage;
        [SerializeField] private MarkerHandlerWithCameraLock markerHandlerForSurvey;

        [Header("Config")]
        [SerializeField] private string newPositionLabel = "new position";
        
        private Action<double,double> newLocationAction;
        private Marker systemMarker;


        public void HandleNewPOI(string name, double lat, double lon, Action<double,double> newLocationAction)
        {
            this.newLocationAction = newLocationAction;

            markerHandlerForSurvey.RemoveAllMarkersAndUnlockCamera();
            systemMarker = markerHandlerForSurvey.AddAPIMarker(name,lat,lon);
            markerHandlerForSurvey.LockCamera(systemMarker);
        }
        public void ResetView()
        {
            markerHandlerForSurvey.RemoveAllMarkersAndUnlockCamera();
        }
        public void SetNewPositionLabel(string text)
        {
            newPositionLabel = text;
        }

        public void HandleMapPointerDownEvent(PointerEventData eventData)
        {
            Vector2 markerPosition = CalculateMarkerPosition(eventData.position);
            Marker userMarker = markerHandlerForSurvey.AddUserMarker(newPositionLabel, markerPosition.x, markerPosition.y);

            Vector2d userLatlonVector = markerHandlerForSurvey.GetMarkerLatLonVector(userMarker);
            if (newLocationAction != null)
                newLocationAction.Invoke(userLatlonVector.x,userLatlonVector.y);
        }
        private Vector2 CalculateNormalizedLocalPoint(Vector2 touchPosition)
        {
            Vector2 localPoint = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mapImage.rectTransform, touchPosition, null, out localPoint);

            Vector2 normalizedLocalPoint = new Vector2(
                localPoint.x / mapImage.rectTransform.rect.width,
                localPoint.y / mapImage.rectTransform.rect.height);

            return normalizedLocalPoint;
        }
        private Vector2 CalculateCameraSize()
        {
            float halfFieldOfView = mapCamera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            float halfHeightAtDepth = mapCamera.transform.position.y * Mathf.Tan(halfFieldOfView);
            float halfWidthAtDepth = mapCamera.aspect * halfHeightAtDepth;

            return new Vector2(halfWidthAtDepth * 2, halfHeightAtDepth * 2);
        }
        private Vector2 CalculateMarkerPosition(Vector2 touchPosition)
        {
            Vector2 localPoint = CalculateNormalizedLocalPoint(touchPosition);
            Vector2 cameraSize = CalculateCameraSize();

            Vector2 markerAtMapCenterPosition = new Vector2(localPoint.x * cameraSize.x, localPoint.y * cameraSize.y);
            //return TranslateMarkerPositionByCameraPosition(markerAtMapCenterPosition);

            return TranslateMarkerPositionByPoiMarker(markerAtMapCenterPosition);
        }
        //private Vector2 TranslateMarkerPositionByCameraPosition(Vector2 markerAtaMapCenterPosition)
        //{
        //    markerAtaMapCenterPosition.x += mapCamera.transform.position.x;
        //    markerAtaMapCenterPosition.y += mapCamera.transform.position.z;

        //    return markerAtaMapCenterPosition;
        //}
        private Vector2 TranslateMarkerPositionByPoiMarker(Vector2 markerAtaMapCenterPosition)
        {
            markerAtaMapCenterPosition.x += systemMarker.GetPosition().x;
            markerAtaMapCenterPosition.y += systemMarker.GetPosition().z;

            return markerAtaMapCenterPosition;
        }
    }
}