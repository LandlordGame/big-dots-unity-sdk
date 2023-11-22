using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Service
{
    public class PlaceProposalStoreReqest
    {
        [JsonProperty("poiName")] public string PoiName { get; set; }
        [JsonProperty("poiCategory")] public string PoiCategory { get; set; }
        [JsonProperty("lat")] public double Lat { get; set; }
        [JsonProperty("lon")] public double Lon { get; set; }

        [JsonProperty("duration")] public int Duration { get; set; }


        public PlaceProposalStoreReqest(string name, string category, double lat, double lon, int duration)
        {
            PoiName = name;
            PoiCategory = category;

            Lat = lat;
            Lon = lon;
        
            Duration = duration;
        }
    }
}
