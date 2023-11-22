using SurveyAPI.Service;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class InfoPanelController : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI infoLabel;
        [SerializeField] private Button closeButton;

        [Header("Events")]
        [SerializeField] private UnityEvent panelCloseEvent;


        public void Show(string text)
        {
            ShowInternal(text,true);
        }
        public void ShowWithoutCloseButton(string text)
        {
            ShowInternal(text,false);
        }

        private void ShowInternal(string text, bool showCloseButton)
        {
            container.SetActive(true);

            if (closeButton != null)
                closeButton.onClick.AddListener(Hide);

            infoLabel.text = text;
            closeButton.gameObject.SetActive(showCloseButton);
        }

        public void Hide()
        {
            container.SetActive(false);

            if (closeButton != null)
                closeButton.onClick.RemoveListener(Hide);

            panelCloseEvent.Invoke();
        }
    }
}