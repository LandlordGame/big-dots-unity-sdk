using Newtonsoft.Json;

namespace SurveyAPI.Service
{   
    [UnityEngine.Scripting.Preserve]
    public class SurveyStatusResponse
    {
        [JsonConstructor]
        [UnityEngine.Scripting.Preserve]
        public SurveyStatusResponse(string userID, bool isSurveyPossible, bool blackList, object nextSurveyDate, object durationToNextSurvey)
        {
            UserID = userID;
            IsSurveyPossible = isSurveyPossible;
            BlackList = blackList;
            NextSurveyDate = nextSurveyDate;
            DurationToNextSurvey = durationToNextSurvey;
        }

        [JsonProperty("userID")] public string UserID { get; set; }

        [JsonProperty("isSurveyPossible")] public bool IsSurveyPossible { get; set; }

        [JsonProperty("blackList")] public bool BlackList { get; set; }

        [JsonProperty("nextSurveyDate")] public object NextSurveyDate { get; set; }

        [JsonProperty("durationToNextSurvey")] public object DurationToNextSurvey { get; set; }
    }
}
