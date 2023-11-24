using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyAPI.Service
{
    public interface SurveyService
    {
        public Task<ServerResponse<SurveyResponse>> GetSurveyAsync(double userLat, double userLon, double surveyLat, double surveyLon);
        
        public Task<ServerResponse<string>> PostSurveyAsync(double userLat, double userLon, SurveyStoreRequest survey);
        public Task<ServerResponse<string>> PostSurveySavePhotosAsync(double userLat, double userLon, string surveyId, IList<byte[]> photos);

        public Task<ServerResponse<string>> PostPlaceProposalAsync(double userLat, double userLon, PlaceProposalStoreReqest storeData);
        public Task<ServerResponse<string>> PostPlaceProposalSavePhotosAsync(double userLat, double userLon, string placeProposalId, IList<byte[]> photos);
        public Task<ServerResponse<SurveyStatusResponse>> GetSurveyStatusAsync(string userID);
        void SetUserID(string userID);
    }
}