using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SurveyAPI.Service
{
    public class SurveyServiceDefault : SurveyServiceBase
    {
        [Header("Keys")]
        [SerializeField] private string apiKey;
        [SerializeField] private string mobileAppId;
        [SerializeField] private string userId;
        [SerializeField] private bool autoGenerateUserId = false;

        [Header("Server")]
        [SerializeField] private string apiHostname;
        [SerializeField] private string getSurveyPath;
        [SerializeField] private string postSurveyPath;
        [SerializeField] private string postSurveyPhotosPath;
        [SerializeField] private string postPlaceProposalPath;
        [SerializeField] private string postPlaceProposalPhotosPath;
        [SerializeField] private string getSurveyStatusPath;

        [Header("Debug")]
        [SerializeField] private bool enableDebugFile = false;
        [SerializeField] private string debugFilePath = "SurveyServiceLog.txt";

        public override void SetUserID(string userID)
        {
            userId = userID;
        }

        private string GenerateUserId()
        {
            return "[sdk-test][" + SystemInfo.deviceModel + "][" + SystemInfo.deviceUniqueIdentifier + "]";
        }

        public override async Task<ServerResponse<SurveyResponse>> GetSurveyAsync(double userLat, double userLon,
            double surveyLat, double surveyLon)
        {
            string url = PrepareGetSurveyUrl(surveyLat, surveyLon);

            return await SendDataAsync<SurveyResponse>(HttpMethod.Get, url, userLat, userLon, null);
        }
        public override async Task<ServerResponse<string>> PostSurveyAsync(double userLat, double userLon, SurveyStoreRequest survey)
        {
            return await SendDataAsync<string>(HttpMethod.Post, apiHostname + postSurveyPath, userLat, userLon, MakeJsonContent(survey));
        }
        public override async Task<ServerResponse<string>> PostSurveySavePhotosAsync(double userLat, double userLon,
            string surveyId, IList<byte[]> photos)
        {
            if (string.IsNullOrEmpty(surveyId) || photos == null || photos.Count == 0)
                throw new Exception("SurveyServiceDefault - surveyId and photos array cannot be null or empty!");

            HttpContent content = MakeMultipartContent(ConvertByteArrayToFileContent("photos", photos),
                new KeyValuePair<string, string>("surveyId", surveyId));

            return await SendDataAsync<string>(HttpMethod.Post, apiHostname + postSurveyPhotosPath, userLat, userLon, content);
        }
        public override async Task<ServerResponse<string>> PostPlaceProposalAsync(double userLat, double userLon,
            PlaceProposalStoreReqest storeData)
        {
            return await SendDataAsync<string>(HttpMethod.Post, apiHostname + postPlaceProposalPath, userLat, userLon, MakeJsonContent(storeData));
        }
        public override async Task<ServerResponse<string>> PostPlaceProposalSavePhotosAsync(double userLat, double userLon,
            string placeProposalId, IList<byte[]> photos)
        {
            if (string.IsNullOrEmpty(placeProposalId) || photos == null || photos.Count == 0)
                throw new Exception("SurveyServiceDefault - placeProposalId and photos array cannot be null or empty!");

            HttpContent content = MakeMultipartContent(ConvertByteArrayToFileContent("photos", photos),
                new KeyValuePair<string, string>("placeProposalId", placeProposalId));

            return await SendDataAsync<string>(HttpMethod.Post, apiHostname + postPlaceProposalPhotosPath, userLat, userLon, content);
        }

        private string PrepareGetSurveyUrl(double lat, double lon)
        {
            string urlParams = $"?lat={lat}&lon={lon}";
            urlParams = ReplaceCommasWithDots(urlParams);
            string urlWithParams = $"{apiHostname}{getSurveyPath}{urlParams}";

            return urlWithParams;
        }
        private string ReplaceCommasWithDots(string value)
        {
            return value.Replace(',', '.');
        }

        private HttpContent MakeJsonContent(object dataToSend)
        {
            if (dataToSend != null)
            {
                string serializedObject = JsonConvert.SerializeObject(dataToSend);
                StringContent contentToSend = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                return contentToSend;
            }
            else
                return null;
        }

        private HttpContent MakeMultipartContent(IList<SurveyServiceFileContent> files, params KeyValuePair<string, string>[] keyValuePairs)
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();

            foreach (KeyValuePair<string, string> pair in keyValuePairs)
                formData.Add(new StringContent(pair.Value), pair.Key);

            foreach (SurveyServiceFileContent file in files)
            {
                ByteArrayContent byteContent = new ByteArrayContent(file.Data);
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                formData.Add(byteContent, file.ContentName, file.FileName);
            }

            return formData;
        }
        private SurveyServiceFileContent[] ConvertByteArrayToFileContent(string contentName, IList<byte[]> data)
        {
            SurveyServiceFileContent[] result = new SurveyServiceFileContent[data.Count];

            for (int i = 0; i < result.Length; i++)
                result[i] = new SurveyServiceFileContent(data[i], contentName, $"file{i}.dat");

            return result;
        }

        private async Task<ServerResponse<T>> SendDataAsync<T>(HttpMethod method, string url,
            double? userLat = null, double? userLon = null, HttpContent contentToSend = null) where T : class
        {
            Debug.Log($"({method}) SendDataAsync<T> - {url}");
            string customMessage = "";

            try
            {
                using HttpClient client = new HttpClient();
                AddHeaders(client, userLat, userLon);

                using HttpRequestMessage request = new HttpRequestMessage(method, url);
                if (contentToSend != null) request.Content = contentToSend;

                using HttpResponseMessage response = await client.SendAsync(request);
                Debug.Log("Response status code: " + response.StatusCode);

                if (enableDebugFile == true)
                    SaveLastRequestToDebugFile(request);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.Log("Response content: " + content);

                    T responseData;
                    if (typeof(T) == typeof(string))
                        responseData = content as T;
                    else
                        responseData = JsonConvert.DeserializeObject<T>(content);

                    return new ServerResponse<T>(response.StatusCode, customMessage, responseData);
                }
                else
                {
                    return new ServerResponse<T>(response.StatusCode, customMessage);
                }
            }
            catch (Exception e)
            {
                customMessage = e.Message;

                Debug.Log(e.ToString());
            }

            return new ServerResponse<T>(System.Net.HttpStatusCode.BadRequest, customMessage);
        }

        private void AddHeaders(HttpClient client, double? userLat, double? userLon)
        {
            if (string.IsNullOrEmpty(mobileAppId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(userId))
                throw new Exception("SurveyServiceDefault - mobileAppId, apiKey and userId cannot be null or empty!");

            client.DefaultRequestHeaders.Add("mobileAppId", mobileAppId);
            client.DefaultRequestHeaders.Add("apiKey", apiKey);
            client.DefaultRequestHeaders.Add("userId", userId);

            if (userLat.HasValue) client.DefaultRequestHeaders.Add("userLat", ReplaceCommasWithDots(userLat.ToString()));
            if (userLon.HasValue) client.DefaultRequestHeaders.Add("userLon", ReplaceCommasWithDots(userLon.ToString()));
        }

        private void SaveLastRequestToDebugFile(HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(debugFilePath) == true)
                return;

            try
            {
                StreamWriter writer = new StreamWriter(debugFilePath, false);
                writer.WriteLine("METHOD");
                writer.WriteLine(request.Method.Method);
                writer.WriteLine("URI");
                writer.WriteLine(request.RequestUri);
                writer.WriteLine("CONTENT");
                writer.WriteLine(request.Content);
                writer.WriteLine("HEADERS");
                writer.WriteLine(request.Headers.ToString());
                writer.Close();

            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public override async Task<ServerResponse<SurveyStatusResponse>> GetSurveyStatusAsync(string userID)
        {
            return await SendDataAsync<SurveyStatusResponse>(HttpMethod.Get, apiHostname + "user/surveyStatus?userId=" + userID);
        }
    }
}
