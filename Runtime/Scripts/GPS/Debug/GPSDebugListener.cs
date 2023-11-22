using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public class GPSDebugListener : MonoBehaviour, GPSListener
    {
        [SerializeField] private GPSServiceBase gps;

        private void OnEnable()
        {
            gps.AddListener(this);
        }
        private void OnDisable()
        {
            gps.RemoveListener(this);
        }

        public void HandleGPSData(GPSData data)
        {
            Debug.Log("GPSDebugListener.HandleGPSData - " + data);
        }
    }
}
