using Newtonsoft.Json;

namespace SurveyAPI.Service
{
    public class SurveyResponse
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("poiName")] public string PoiName { get; set; }
        [JsonProperty("poiCategory")] public string[] PoiCategory { get; set; }
        [JsonProperty("lat")] public double Lat { get; set; }
        [JsonProperty("lon")] public double Lon { get; set; }
        [JsonProperty("placeProposalId")] public string PlaceProposalId { get; set; }

        /// <summary>
        /// List containing names of all possible categories that a venue can have - use for setting up category correction flow
        /// where all categories should be shown to the user.
        /// </summary>
        [JsonProperty("availableCategories")] public string[] PossibleCategories { get; set; }
    }
}