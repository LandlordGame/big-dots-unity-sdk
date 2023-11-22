using SurveyAPI.Shared;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class SurveyPanelViewControllerDefault : SurveyPanelViewControllerBase
    {
        [Header("Canvas")]
        [SerializeField] TextMeshProUGUI titleLabel;
        [SerializeField] TMP_InputField nameInputField;
        [SerializeField] TMP_InputField categoryInputField;
        [SerializeField] TextMeshProUGUI changePositionButtonLabel;
        [SerializeField] TextMeshProUGUI sendButtonLabel;
        [SerializeField] TextMeshProUGUI closeButtonLabel;

        [Header("Config")]
        [SerializeField] string titleLabelText = "Panel title";
        [SerializeField] string sendButtonDefaultText = "Send";
        [SerializeField] string closeButtonText = "Close without sending";
        [SerializeField] string changingPositionOffLabel = "Click here to choose position";
        [SerializeField] string changingPositionOnLabel = "[click on map to choose position]";        

        [Header("Config for data validation")]
        [SerializeField] int minimumPhotosCount = 1;
        [SerializeField] string emptyNameText = "[Name cannot be empty!]";
        [SerializeField] string emptyCategoryText = "[Category cannot be empty!]";
        [SerializeField] string emptyPositionText = "[Choose the position of your place!]";
        [SerializeField] string emptyPhotosText = "[Add at least one photo!]";

        [Header("Config for IncorrectDataAction")]
        [SerializeField] bool enableIncorrectDataAction = true;
        [SerializeField] Color incorrectDataTextColor = new Color(0.75f,0,0,1);
        [SerializeField] float incorrectDataMessageTimeInSeconds = 3;


        public override void ResetView()
        {
            nameInputField.text = "";
            categoryInputField.text = "";
            
            sendButtonLabel.text = sendButtonDefaultText;
            closeButtonLabel.text = closeButtonText;
            changePositionButtonLabel.text = changingPositionOffLabel;
        }
        public override void SetTitleLabel(string text)
        {
            titleLabel.text = text;
        }
        public override void SetChangePositionButtonLabel(bool changingPositionOn)
        {
            if (changingPositionOn == true)
                changePositionButtonLabel.text = changingPositionOnLabel; else
                changePositionButtonLabel.text = changingPositionOffLabel;
        }
        public override void SetNameInputText(string text) { nameInputField.text = text; }
        public override string GetNameInputText() { return nameInputField.text; }
        public override void SetCategoryInputText(string text) { categoryInputField.text = text; }
        public override string GetCategoryInputText() { return categoryInputField.text; }

        public override bool ValidateData(PositionDouble placePosition, int addedPhotosCount, out string incorrectDataMessage)
        {
            bool result = false;
            string msg = "";

            string name = nameInputField.text;
            string category = categoryInputField.text;

            if (name.Length == 0)
                msg = emptyNameText; else
            if (category.Length == 0)
                msg = emptyCategoryText; else
            if (placePosition == null)
                msg = emptyPositionText; else
            if (addedPhotosCount < minimumPhotosCount)
                msg = emptyPhotosText; else
                result = true;

            if (result == false)
            {
                if (enableIncorrectDataAction == true)
                    StartCoroutine(IncorrectDataAction(msg));
            }

            incorrectDataMessage = msg;

            return result;
        }
        private IEnumerator IncorrectDataAction(string text)
        {
            Color previousColor = sendButtonLabel.color;

            sendButtonLabel.text = text;
            sendButtonLabel.color = incorrectDataTextColor;

            yield return new WaitForSecondsRealtime(incorrectDataMessageTimeInSeconds);

            sendButtonLabel.text = sendButtonDefaultText;
            sendButtonLabel.color = previousColor;
        }    
    }
}
