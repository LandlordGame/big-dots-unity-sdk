using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class SurveyPanelInputHandler : MonoBehaviour
    {
        [Header("Mapbox event provider")]
        [SerializeField] TouchEventProvider mapboxEventProvider;

        [Header("Canvas")]
        [SerializeField] Button sendButton;
        [SerializeField] Button closeButton;
        [SerializeField] Button changePositionButton;
        [SerializeField] Button[] photoButtons;

        public void Register(UnityAction<PointerEventData> mapboxTouchAction, UnityAction sendButtonAction, UnityAction closeButtonAction,
            UnityAction changePositionButtonAction, UnityAction<int> photoButtonAction)
        {
            Unregister();

            mapboxEventProvider.pointerDownEvent.AddListener(mapboxTouchAction);
           
            sendButton.onClick.AddListener(sendButtonAction);
            closeButton.onClick.AddListener(closeButtonAction);
            changePositionButton.onClick.AddListener(changePositionButtonAction);

            for (int i =0; i < photoButtons.Length; i++)
            {
                int index = i;
                photoButtons[i].onClick.AddListener(() => { photoButtonAction(index); });
            }
        }
        public void Unregister()
        {
            mapboxEventProvider.pointerDownEvent.RemoveAllListeners();

            sendButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            changePositionButton.onClick.RemoveAllListeners();

            for (int i =0; i < photoButtons.Length; i++)
                photoButtons[i].onClick.RemoveAllListeners();
        }
    }
}