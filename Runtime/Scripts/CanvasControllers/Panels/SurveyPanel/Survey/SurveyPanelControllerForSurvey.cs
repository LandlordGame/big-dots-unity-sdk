using SurveyAPI.Service;
using SurveyAPI.Shared;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SurveyAPI.CanvasControllers
{
    public class SurveyPanelControllerForSurvey : MonoBehaviour
    {
        [Header("External")]
        [SerializeField][ReqField] RequestHandlerForSurveyBase requestHandler;
        [SerializeField][ReqField] SurveyPanelLocationControllerBase locationController;
        [SerializeField][ReqField] SurveyPanelViewControllerBase viewController;
        [SerializeField][ReqField] PhotoCameraGuiControllerBase photoController;
        [SerializeField][ReqField] SurveyPanelInputHandler inputHandler;

        [Header("Canvas")]
        [SerializeField] GameObject container;

        [Header("Config")]
        [SerializeField] string waitingForServerResponseText = "Waiting for server response..";

        [Header("Events")]
        [SerializeField] UnityEvent panelCloseWithoutSendingEvent;
        [SerializeField] UnityEvent<string> waitingForServerResponseEvent;

        private SurveyResponse survey;
        private float duration;


        private void Update()
        {
            if (IsPanelOpened() == true)
                duration += Time.deltaTime;
        }
        private bool IsPanelOpened()
        {
            return container.activeSelf;
        }


        public void ShowSurvey(SurveyResponse survey)
        {
            if (survey == null)
                throw new System.Exception("SurveyPanelController - survey cannot be null!");

            this.survey = survey;

            container.SetActive(true);

            inputHandler.Register(HandleMapPointerDownEvent,HandleSendButton,HandleCloseButton,
                HandleChoosePositionButton,HandlePhotoImageButton);

            duration = 0;
            viewController.ResetView();
            photoController.RemovePhotos();

            FillViewWithSurveyData();

            locationController.Show(survey.PoiName,survey.Lat,survey.Lon);
        }
        private void FillViewWithSurveyData()
        {
            viewController.SetTitleLabel(survey.Id);
            viewController.SetNameInputText(survey.PoiName);
            viewController.SetCategoryInputText(MergeStringArray(survey.PoiCategory));
        }
        private string MergeStringArray(string[] array)
        {
            if (array == null || array.Length == 0)
                return "";

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                builder.Append(array[i]);

                if (i <= array.Length - 2)
                    builder.Append(",");
            }

            return builder.ToString();
        }


        public void Hide()
        {
            inputHandler.Unregister();

            container.SetActive(false);

            locationController.Hide();
            photoController.RemovePhotos();
        }

        public void HandleTakePhotoButton()
        {
            photoController.OpenPhotoCamera(0);
        }
        public void HandlePhotoImageButton(int index)
        {
            if (photoController.DoesPhotoExist(index) == false)
                photoController.OpenPhotoCamera(index); else
                photoController.RemovePhoto(index);
        }

        public void HandleChoosePositionButton()
        {
            ToggleChangingPosition();
        }
        private void ToggleChangingPosition()
        {
            locationController.ToggleChangingPositionMode();

            viewController.SetChangePositionButtonLabel(locationController.GetChangingPositionMode());
        }
        public void HandleMapPointerDownEvent(PointerEventData eventData)
        {
            if (locationController.GetChangingPositionMode() == true)
            {
                locationController.HandleMapPointerDownEvent(eventData);
                ToggleChangingPosition();
            }
        }

        public void HandleSendButton()
        {
            SendStoreRequest();
        }

        private void SendStoreRequest()
        {
            SurveyStoreRequest surveyStoreRequest = PrepareSurveyStoreRequest();
            PositionDouble lastUserPosition = this.locationController.GetLastUserPosition();

            requestHandler.PostSurvey(lastUserPosition.Lat,lastUserPosition.Lon,surveyStoreRequest,photoController.TrimAndEncodeTextureArray());

            Hide();

            waitingForServerResponseEvent.Invoke(waitingForServerResponseText);
        }
        private SurveyStoreRequest PrepareSurveyStoreRequest()
        {
            string surveyID = survey.Id;
            string placeProposalID = survey.PlaceProposalId;
            string name = viewController.GetNameInputText();
            string category = viewController.GetCategoryInputText();
            
            PositionDouble position = locationController.GetPlacePosition();
            if (position == null)
                position = new PositionDouble(survey.Lat,survey.Lon);
            
            bool nameChanged = false;
            bool categoryChanged = false;
            bool positionChanged = false;

            if (survey.PoiName.Equals(name) == false)
                nameChanged = true;
            if (MergeStringArray(survey.PoiCategory).Equals(category) == false)
                categoryChanged = true;
            if (position.Lat != survey.Lat || position.Lon != survey.Lon)
                positionChanged = true;
            
            SurveyStoreRequest request = new SurveyStoreRequest(
                surveyID,
                placeProposalID,
                name,nameChanged,
                category,categoryChanged,
                position.Lat,position.Lon,positionChanged,
                Mathf.CeilToInt(duration));

            return request;
        }

        public void HandleCloseButton()
        {
            Hide();

            panelCloseWithoutSendingEvent.Invoke();
        }
    }
}
