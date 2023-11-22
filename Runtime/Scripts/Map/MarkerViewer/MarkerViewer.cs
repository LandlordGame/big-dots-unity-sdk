using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Map
{
    public interface MarkerViewer
    {
        public Marker AddMarker(string text, float worldX, float worldZ, MarkerType type);
        public bool RemoveMarker(Marker marker);
    }
}
