using SurveyAPI.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SurveyAPI.CanvasControllers
{
    public abstract class SurveyPanelLocationControllerBase : MonoBehaviour, SurveyPanelLocationController
    {
        public abstract void Show(string poiName, double lat, double lon);
        public abstract void Show();
        public abstract void Hide();

        public abstract void ToggleChangingPositionMode();
        public abstract bool GetChangingPositionMode();
        public abstract void UpdatePlacePositionLabel(string text);
        public abstract PositionDouble GetPlacePosition();
        public abstract PositionDouble GetLastUserPosition();

        public abstract void HandleMapPointerDownEvent(PointerEventData eventData);
    }
}
