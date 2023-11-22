using SurveyAPI.CanvasControllers;
using SurveyAPI.GPS;
using SurveyAPI.Map;
using SurveyAPI.Service;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class NavigationPanelController : MonoBehaviour
    {
        [Header("External")]
        [SerializeField] private SurveyServiceBase surveyService; 
        [SerializeField] private GPSServiceBase gps;

        [Header("Canvas")]
        [SerializeField] private GameObject container;
        [SerializeField] private Button getSurveyButton;
        [SerializeField] private Button placeProposalButton;
        [SerializeField] private TextMeshProUGUI getSurveyButtonLabel;

        [Header("Config")]
        [SerializeField] private bool hideAfterGettingSurvey = true;
        [SerializeField] private string getSurveyButtonBeforeClickText = "Get Survey";
        [SerializeField] private string getSurveyButtonAfterClickText = "Gettting survey, please wait...";
        [SerializeField][TextArea(3,5)] private string problemWithSurveyText = "Server status code: {0}\n{1}";

        [Header("Events")]
        [SerializeField] private UnityEvent<SurveyResponse> surveyResponseEvent;
        [SerializeField] private UnityEvent<string> problemWithGettingSurveyEvent;
        [SerializeField] private UnityEvent placeProposalButtonEvent;


        public void Show()
        {
            container.SetActive(true);

            if (getSurveyButton != null)
                getSurveyButton.onClick.AddListener(HandleGetSurveyButton);
            if (placeProposalButton != null)
                placeProposalButton.onClick.AddListener(HandlePlaceProposalButton);

            EnableGetSurveyButton();
            EnablePlaceProposalButton();
        }
        public void Hide()
        {
            container.SetActive(false);
            
            if (getSurveyButton != null)
                getSurveyButton.onClick.RemoveListener(HandleGetSurveyButton);
            if (placeProposalButton != null)
                placeProposalButton.onClick.RemoveListener(HandlePlaceProposalButton);
        }

        private void EnableGetSurveyButton()
        {
            getSurveyButton.interactable = true;
            getSurveyButtonLabel.text = getSurveyButtonBeforeClickText;
        }
        private void DisableGetSurveyButton()
        {
            getSurveyButton.interactable = false;
            getSurveyButtonLabel.text = getSurveyButtonAfterClickText;
        }

        private void EnablePlaceProposalButton()
        {
            placeProposalButton.interactable = true;
        }
        private void DisablePlaceProposalButton()
        {
            placeProposalButton.interactable = false;
        }





        public void HandleGetSurveyButton()
        {
            DisableGetSurveyButton();
            DisablePlaceProposalButton();

            GetSurveyAtLastLocation();
        }
        private void GetSurveyAtLastLocation()
        {
            GPSData lastGPSData = gps.GetLastGPSPosition();

            if (lastGPSData != null)
            {
                // in the current implementation, user position and survey position is always the same
                StartCoroutine(GetSurveyAction(lastGPSData.Lat,lastGPSData.Lon,lastGPSData.Lat,lastGPSData.Lon));
            }
        }
        private IEnumerator GetSurveyAction(double userLat, double userLon, double surveyLat, double surveyLon)
        {
            Task<ServerResponse<SurveyResponse>> task = surveyService.GetSurveyAsync(userLat,userLon,surveyLat,surveyLon);
            while (task.IsCompleted == false)
                yield return null;

            if (hideAfterGettingSurvey == true)
                Hide();

            ServerResponse<SurveyResponse> response = task.Result;
            if (task.IsCompletedSuccessfully == true && task.Result.Value != null)
            {
                SurveyResponse survey = task.Result.Value;
                surveyResponseEvent.Invoke(survey);
            } else
            {
                problemWithGettingSurveyEvent.Invoke(string.Format(problemWithSurveyText,
                    response.StatusCode,response.CustomMessage));
            }
        }

        public void HandlePlaceProposalButton()
        {
            Hide();

            placeProposalButtonEvent.Invoke();
        }
    }
}