using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public class GPSServiceDefault : GPSServiceBase
    {
        [SerializeField] private GPSDataProviderBase dataProvider;
        [SerializeField] private float refreshRateInSeconds = 1;
        [SerializeField] private int positionHistoryLimit = 100;

        private readonly LinkedList<GPSData> positionHistory = new LinkedList<GPSData>();
        private readonly List<GPSListener> listeners = new List<GPSListener>();
        private float refreshCounter;

        void Update()
        {
            if (dataProvider.GetStatus() != GPSStatus.RUNNING)
                return;

            refreshCounter += Time.deltaTime;
            if (refreshCounter >= refreshRateInSeconds)
            {
                refreshCounter -= refreshRateInSeconds;

                GetGPSData();
            }
        }
        private void GetGPSData()
        {
            GPSData data = dataProvider.GetLastPosition();

            UpdateHistory(data);
            SendGPSDataToListeners(data);
        }
        private void UpdateHistory(GPSData data)
        {
            positionHistory.AddFirst(data);
            if (positionHistory.Count > positionHistoryLimit)
                positionHistory.RemoveLast();
        }
        private void SendGPSDataToListeners(GPSData data)
        {
            foreach (GPSListener listener in listeners)
                listener.HandleGPSData(data);
        }

        public override void AddListener(GPSListener listener)
        {
            if (listener != null)
                listeners.Add(listener);
        }
        public override void RemoveListener(GPSListener listener)
        {
            if (listener != null)
                listeners.Remove(listener);
        }
        public override GPSStatus GetState()
        {
            return dataProvider.GetStatus();
        }

        public override GPSData GetLastGPSPosition()
        {
            if (positionHistory.Count > 0)
                return positionHistory.First.Value; else
                return null;
        }
        public override GPSData[] GetGPSHistory(int count)
        {
            if (count <= 0)
                return new GPSData[] { };

            if (count > positionHistory.Count)
                count = positionHistory.Count;

            GPSData[] array = new GPSData[count];
            positionHistory.CopyTo(array,0);

            return array;
        }
    }
}