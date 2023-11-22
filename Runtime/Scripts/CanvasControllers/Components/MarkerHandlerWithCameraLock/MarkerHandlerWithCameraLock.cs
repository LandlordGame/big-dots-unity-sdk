using Mapbox.Unity.Map;
using Mapbox.Utils;
using SurveyAPI.Map;
using SurveyAPI.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public class MarkerHandlerWithCameraLock : MonoBehaviour
    {
        [Header("External")]
        [SerializeField] private AbstractMap mapbox;
        [SerializeField] private MarkerViewerBase markerViewer;
        [SerializeField] private CameraControllerBase cameraController;

        [Header("Config")]
        [SerializeField] private int nameLengthLimit = 25;
        [SerializeField] private float cameraZoomWhenLocked = 2;

        private bool lockOn;
        private Vector3 lastCameraPosition;
        private Marker systemMarker;
        private Marker userMarker;


        public Marker AddAPIMarker(string name, double lat, double lon)
        {
            markerViewer.RemoveMarker(systemMarker);
            systemMarker = AddMarkerLatLon(name,lat,lon,MarkerType.POI_API_MARKER);
            return systemMarker;
        }
        public Marker AddUserMarker(string name, float worldX, float worldZ)
        {
            markerViewer.RemoveMarker(userMarker);
            userMarker = AddMarkerAtWorldPosition(name,worldX,worldZ,MarkerType.POI_USER_MARKER);
            return userMarker;
        }

        public Vector2d GetMarkerLatLonVector(Marker marker)
        {
            return mapbox.WorldToGeoPosition(marker.GetPosition());
        }
        public void RemoveAllMarkersAndUnlockCamera()
        {
            markerViewer.RemoveMarker(systemMarker);
            markerViewer.RemoveMarker(userMarker);

            systemMarker = null;
            userMarker = null;

            UnlockCamera();
        }


        public void LockCamera(Marker marker)
        {
            lastCameraPosition = cameraController.GetCameraPosition();

            Vector3 position = marker.GetPosition();
            position.y = lastCameraPosition.y / cameraZoomWhenLocked;

            cameraController.LockCameraOnPosition(position);
            lockOn = true;
        }
        private void UnlockCamera()
        {
            if (lockOn == false)
                return;

            cameraController.UnlockCameraPosition();

            cameraController.MoveCameraToPosition(lastCameraPosition);
            lockOn = false;
        }


        private Marker AddMarkerLatLon(string text, double lat, double lon, MarkerType type)
        {
            //double surveyLat = double.Parse(survey.Lat,System.Globalization.CultureInfo.InvariantCulture);
            //double surveyLon = double.Parse(survey.Lon,System.Globalization.CultureInfo.InvariantCulture);

            Vector3 position = mapbox.GeoToWorldPosition(new Mapbox.Utils.Vector2d(lat,lon));

            if (text.Length > nameLengthLimit)
                text = text.Substring(0,nameLengthLimit);

            return AddMarkerAtWorldPosition(text, position.x,position.z,type);
        }
        private Marker AddMarkerAtWorldPosition(string text, float x, float z, MarkerType type)
        {
            if (text.Length > nameLengthLimit)
                text = text.Substring(0,nameLengthLimit);

            return markerViewer.AddMarker(text,x,z,type);
        }
    }
}