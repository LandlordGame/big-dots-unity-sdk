using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Map
{
    public class MarkerViewerDefault : MarkerViewerBase
    {
        [SerializeField] private float unitScale = 1f;

        [SerializeField] private InternalMarker[] markerPrefabs;
        [SerializeField] private Transform markersContainer;
        [SerializeField] private int markerLimit = 25;

        private LinkedList<InternalMarker> markers = new LinkedList<InternalMarker>();


        public override Marker AddMarker(string text, float worldX, float worldZ, MarkerType type)
        {
            InternalMarker marker = MakeInternalMarker(text,worldX,worldZ,type);

            markers.AddLast(marker);
            this.CheckMarkerLimit();

            return marker;
        }
        private InternalMarker MakeInternalMarker(string text, float worldX, float worldZ, MarkerType type)
        {
            InternalMarker prefab = markerPrefabs[(int)type];
            
            Vector3 position = new Vector3(worldX,prefab.transform.position.y,worldZ);

            InternalMarker marker = Instantiate<InternalMarker>(prefab,position,Quaternion.identity, markersContainer);
            marker.SetTitle(text);
            marker.SetMainObjectScale(unitScale);

            return marker;
        }
        private void CheckMarkerLimit()
        {
            if (markerLimit > 0 && markers.Count > markerLimit)
                RemoveMarker(markers.First.Value);
        }

        public override bool RemoveMarker(Marker marker)
        {
            InternalMarker markerInternal = marker as InternalMarker;
            if (markerInternal != null)
            {
                bool result = markers.Remove(markerInternal);
                if (result == true)
                    Destroy(markerInternal.gameObject);

                return result;
            }

            return false;
        }
    }
}
