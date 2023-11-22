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
        public string[] PossibleCategories { get; } = new string[] {
            "Arts and Entertainment",
            "Business and Professional Services",
            "Home Improvement Service",
            "Office",
            "Automotive Service",
            "Community and Government",
            "Education",
            "Government Building",
            "Dining and Drinking",
            "Restaurant",
            "Health and Medicine",
            "Landmarks and Outdoors",
            "Park",
            "Retail",
            "Food and Beverage Retail",
            "Sports and Recreation",
            "Travel and Transportation"
         };
    }
}