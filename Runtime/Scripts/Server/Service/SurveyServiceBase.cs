using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SurveyAPI.Service
{
    public abstract class SurveyServiceBase : MonoBehaviour, SurveyService
    {
        public abstract Task<ServerResponse<SurveyResponse>> GetSurveyAsync(double userLat, double userLon, double surveyLat, double surveyLon);
        
        public abstract Task<ServerResponse<string>> PostSurveyAsync(double userLat, double userLon, SurveyStoreRequest survey);
        public abstract Task<ServerResponse<string>> PostSurveySavePhotosAsync(double userLat, double userLon, string surveyId, IList<byte[]> photos);

        public abstract Task<ServerResponse<string>> PostPlaceProposalAsync(double userLat, double userLon, PlaceProposalStoreReqest storeData);
        public abstract Task<ServerResponse<string>> PostPlaceProposalSavePhotosAsync(double userLat, double userLon, string placeProposalId, IList<byte[]> photos);

        public abstract Task<ServerResponse<SurveyStatusResponse>> GetSurveyStatusAsync(string userID);
    }
}