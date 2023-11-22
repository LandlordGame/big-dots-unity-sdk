using SurveyAPI.Service;
using SurveyAPI.Shared;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class SurveyPanelControllerForPlaceProposal : MonoBehaviour
    {
        [Header("External")]
        [SerializeField][ReqField] RequestHandlerForPlaceProposalBase requestHandler;
        [SerializeField][ReqField] SurveyPanelLocationControllerBase locationController;
        [SerializeField][ReqField] SurveyPanelViewControllerBase viewController;
        [SerializeField][ReqField] PhotoCameraGuiControllerBase photoController;
        [SerializeField][ReqField] SurveyPanelInputHandler inputHandler;

        [Header("Canvas")]
        [SerializeField] GameObject container;

        [Header("Config")]
        [SerializeField] string titleTextForPlaceProposal = "Place proposal panel";
        [SerializeField] string waitingForServerResponseText = "Waiting for server response..";

        [Header("Events")]
        [SerializeField] UnityEvent panelCloseWithoutSendingEvent;
        [SerializeField] UnityEvent<string> waitingForServerResponseEvent;

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

        public void Show()
        {
            container.SetActive(true);

            inputHandler.Register(HandleMapPointerDownEvent,HandleSendButton,HandleCloseButton,
                HandleChoosePositionButton,HandlePhotoImageButton);

            duration = 0;
            viewController.ResetView();
            photoController.RemovePhotos();

            locationController.Show();
            viewController.SetTitleLabel(titleTextForPlaceProposal);
            viewController.SetNameInputText("");
            viewController.SetCategoryInputText("");
        }

        public void Hide()
        {
            inputHandler.Unregister();

            container.SetActive(false);

            locationController.Hide();
            photoController.RemovePhotos();
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
            HandleSendForPlaceProposal();
        }
        private void HandleSendForPlaceProposal()
        {
            bool dataOK = viewController.ValidateData(locationController.GetPlacePosition(), photoController.GetAddedPhotoCount(),
                out string incorrectDataMessage);

            if (dataOK == true)
                SendStoreRequest();
        }

        private void SendStoreRequest()
        {
            PlaceProposalStoreReqest data = PrepareStoreRequest();
            PositionDouble lastUserPosition = this.locationController.GetLastUserPosition();

            requestHandler.PostPlaceProposal(lastUserPosition.Lat,lastUserPosition.Lon,data,photoController.TrimAndEncodeTextureArray());

            Hide();

            waitingForServerResponseEvent.Invoke(waitingForServerResponseText);
        }
        private PlaceProposalStoreReqest PrepareStoreRequest()
        {
            PositionDouble placePosition = locationController.GetPlacePosition();
            if (placePosition == null)
                throw new Exception(GetType().Name + " - place position cannot be null");

            PlaceProposalStoreReqest data = new PlaceProposalStoreReqest(
                viewController.GetNameInputText(),
                viewController.GetCategoryInputText(),
                placePosition.Lat,placePosition.Lon,
                Mathf.CeilToInt(duration));

            return data;
        }


        public void HandleCloseButton()
        {
            Hide();

            panelCloseWithoutSendingEvent.Invoke();
        }
    }
}
