using Mapbox.Unity.Map;
using Mapbox.Utils;
using SurveyAPI.GPS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Map
{
    public class LocationViewerDefault : LocationViewerBase, GPSListener
    {
        [SerializeField] private GPSServiceBase gps;
        [SerializeField] private MarkerViewerBase markerViewer;
        [SerializeField] private AbstractMap mapbox;
        [SerializeField] private CameraControllerBase cameraController;

        [SerializeField] private int markerHistoryLimit = 10;
        [SerializeField] private string playerMarkerLabel = "player";

        private readonly LinkedList<Marker> lastMarkers = new LinkedList<Marker>();


        private void OnEnable()
        {
            gps.AddListener(this);

        }
        private void OnDisable()
        {
            gps.RemoveListener(this);
        }

        public void HandleGPSData(GPSData data)
        {
            if (mapbox.isActiveAndEnabled == false)
                return;

            DeactivateLastMarker();

            Marker marker = MakeMarker(playerMarkerLabel,data.Lat,data.Lon,MarkerType.PLAYER_ACTIVE_MARKER);

            UpdateCameraPosition(marker);
            AddMarker(marker);
        }
        private void DeactivateLastMarker()
        {
            if (lastMarkers.Count == 0)
                return;

            Marker lastMarker = lastMarkers.Last.Value;
            
            markerViewer.RemoveMarker(lastMarker);
            Marker inactiveMarker = markerViewer.AddMarker("",
                lastMarker.GetPosition().x,lastMarker.GetPosition().z,MarkerType.PLAYER_INACTIVE_MARKER);

            lastMarkers.RemoveLast();
            lastMarkers.AddLast(inactiveMarker);
        }
        private Marker MakeMarker(string text, double lat, double lon, MarkerType type)
        {
            Vector3 position = mapbox.GeoToWorldPosition(new Mapbox.Utils.Vector2d(lat,lon));
            return markerViewer.AddMarker(text,position.x,position.z,type);
        }
        private void UpdateCameraPosition(Marker marker)
        {
            Vector3 newPosition = marker.GetPosition();
            newPosition.y = cameraController.GetCameraPosition().y;

            cameraController.MoveCameraToPosition(newPosition);
        }
        private void AddMarker(Marker marker)
        {
            lastMarkers.AddLast(marker);

            CheckHistoryLimit();
        }
        private void CheckHistoryLimit()
        {
            if (lastMarkers.Count >= markerHistoryLimit)
            {
                markerViewer.RemoveMarker(lastMarkers.First.Value);
                lastMarkers.RemoveFirst();
            }
        }
    }
}
