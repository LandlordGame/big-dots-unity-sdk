using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Map
{
    public abstract class MarkerViewerBase : MonoBehaviour, MarkerViewer
    {
        public abstract Marker AddMarker(string text, float worldX, float WorldZ, MarkerType type);
        public abstract bool RemoveMarker(Marker marker);
    }
}
