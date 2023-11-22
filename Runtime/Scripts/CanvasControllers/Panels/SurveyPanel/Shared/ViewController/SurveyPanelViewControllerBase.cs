using SurveyAPI.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public abstract class SurveyPanelViewControllerBase : MonoBehaviour, SurveyPanelViewController
    {
        public abstract void ResetView();
        
        public abstract void SetTitleLabel(string text);
        public abstract void SetChangePositionButtonLabel(bool changingPositionOn);
        
        public abstract void SetNameInputText(string text);
        public abstract string GetNameInputText();
        public abstract void SetCategoryInputText(string text);
        public abstract string GetCategoryInputText();

        public abstract bool ValidateData(PositionDouble placePosition, int addedPhotosCount, out string incorrectDataMessage);
    }
}
