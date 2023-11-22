using SurveyAPI.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SurveyAPI.CanvasControllers
{
    public interface SurveyPanelLocationController
    {
        public void Show(string poiName, double lat, double lon);
        public void Show();
        public void Hide();

        public void ToggleChangingPositionMode();
        public abstract bool GetChangingPositionMode();
        public void UpdatePlacePositionLabel(string text);
        public PositionDouble GetPlacePosition();
        public PositionDouble GetLastUserPosition();

        public void HandleMapPointerDownEvent(PointerEventData eventData);
    }
}
