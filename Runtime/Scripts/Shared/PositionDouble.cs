using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Shared
{
    public class PositionDouble
    {
        public double Lat { get; private set; }
        public double Lon { get; private set; }

        public PositionDouble(double lat, double lon)
        {
            this.Lat = lat;
            this.Lon = lon;
        }
    }
}
