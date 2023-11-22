using SurveyAPI.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public interface SurveyPanelViewController
    {
        public void ResetView();
        
        public void SetTitleLabel(string text);
        public void SetChangePositionButtonLabel(bool changingPositionOn);
        
        public void SetNameInputText(string text);
        public string GetNameInputText();
        public void SetCategoryInputText(string text);
        public string GetCategoryInputText();

        public bool ValidateData(PositionDouble placePosition, int addedPhotosCount, out string incorrectDataMessage);
    }
}
