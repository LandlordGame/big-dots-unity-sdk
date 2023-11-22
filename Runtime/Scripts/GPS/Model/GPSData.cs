using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.GPS
{
    public class GPSData
    {
        public long TimeStamp { get; private set; }
        public double Lat { get; private set; }
        public double Lon { get; private set; }

        public GPSData(long timeStamp, double lat, double lon)
        {
            this.TimeStamp = timeStamp;
            this.Lat = lat;
            this.Lon = lon;
        }
        public override string ToString()
        {
            return $"GPSData (lat: {Lat}, lon: {Lon})";
        }
    }
}
