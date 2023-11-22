using Newtonsoft.Json;

namespace SurveyAPI.Service
{
    public class SurveyStatusResponse
    {
        [JsonProperty("userID")] public string UserID { get; set; }

        [JsonProperty("isSurveyPossible")] public bool IsSurveyPossible { get; set; }

        [JsonProperty("blackList")] public bool BlackList { get; set; }

        [JsonProperty("nextSurveyDate")] public object NextSurveyDate { get; set; }

        [JsonProperty("durationToNextSurvey")] public object DurationToNextSurvey { get; set; }
    }
}