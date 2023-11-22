using Mapbox.Unity.Map;
using SurveyAPI.GPS;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingPanelController : MonoBehaviour
{
    private enum LoadingState
    {
        LOADING_NOT_STARTED,
        LOADING_STARTED,
        LOADING_COMPLETED
    }

    [Header("External")]
    [SerializeField] private GPSServiceBase gps;
    [SerializeField] private AbstractMap mapbox;

    [Header("Canvas")]
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Button refreshButton;

    [Header("Config")]
    [SerializeField] private string gpsStoppedInfo = "Waiting for GPS data...";
    [SerializeField] private string gpsInitInfo = "GPS is initializing...";
    [SerializeField] private string gpsWorkingInfo = "GPS is working";
    [SerializeField] private string gpsFailedInfo = "Enable GPS in your settings";
    [SerializeField] private float refreshInterval = 1;
    
    [Header("Events")]
    [SerializeField] private UnityEvent loadingCompleteEvent;
    
    private float time;
    private bool gpsFound = false;


    private void Start()
    {
        Show();
    }

    public void Show()
    {
        container.SetActive(true);

        if (refreshButton != null)
            refreshButton.onClick.AddListener(HandleRefreshButton);

        gpsFound = false;
    }
        
    private void Hide()
    {
        container.SetActive(false);

        if (refreshButton != null)
            refreshButton.onClick.RemoveListener(HandleRefreshButton);
    }
    private void SetText(string text)
    {
        label.text = text;
    }
    private void Update()
    {
        if (container.activeSelf == false || gpsFound == true)
            return;

        time += Time.deltaTime;
        if (time >= refreshInterval)
        {
            time -= refreshInterval;

            GPSStatus gpsStatus = gps.GetState();
            Debug.Log("gpsStatus: " + gpsStatus);
            SetTextBasedOnStatus(gpsStatus);

            if (gpsStatus == GPSStatus.RUNNING)
            {
                gpsFound = true;
                LoadMap();
            }
        }
    }
    private void SetTextBasedOnStatus(GPSStatus gpsStatus)
    {
        if (gpsStatus == GPSStatus.STOPPED)
            SetText(gpsStoppedInfo); else
        if (gpsStatus == GPSStatus.INITIALIZING)
            SetText(gpsInitInfo); else
        if (gpsStatus == GPSStatus.RUNNING)
            SetText(gpsWorkingInfo); else
        if (gpsStatus == GPSStatus.FAILED)
            SetText(gpsFailedInfo);
    }

    private void LoadMap()
    {
        StartCoroutine(LoadMapAction());
    }
    private IEnumerator LoadMapAction()
    { 
        while (gps.GetLastGPSPosition() == null)
            yield return null;

        InitMapbox();

        while (mapbox.isActiveAndEnabled == false)
            yield return null;

        Hide();
        loadingCompleteEvent.Invoke();
    }
    private void InitMapbox()
    {
        GPSData gpsData = gps.GetLastGPSPosition();
        mapbox.Initialize(new Mapbox.Utils.Vector2d(gpsData.Lat,gpsData.Lon),(int)mapbox.Zoom);
    }

    public void HandleRefreshButton()
    {

    }
}
