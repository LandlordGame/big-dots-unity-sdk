using Newtonsoft.Json;

public class SurveyStoreRequest
{
    public class PhotoInfo
    {
        [JsonProperty("gyroX")] public double GyroX { get; set; }
        [JsonProperty("gyroY")] public double GyroY { get; set; }
        [JsonProperty("gyroZ")] public double GyroZ { get; set; }
        [JsonProperty("longitude")] public double Lat { get; set; }
        [JsonProperty("latitude")] public double Lon { get; set; }
    }

    public class DeviceInfo
    {
        [JsonProperty("deviceModel")] public string DeviceModel { get; set; }
        [JsonProperty("osVersion")] public string OsVersion { get; set; }
        [JsonProperty("cameraDetails")] public string CameraDetails { get; set; }
    }

    [JsonProperty("poiId")] public string PoiId { get; set; }
    [JsonProperty("placeProposalId")] public string PlaceProposalId { get; set; }

    [JsonProperty("poiName")] public string PoiName { get; set; }
    [JsonProperty("isPoiNameChanged")] public bool IsPoiNameChanged { get; set; }

    [JsonProperty("poiCategory")] public string PoiCategory { get; set; }
    [JsonProperty("isPoiCategoryChanged")] public bool IsPoiCategoryChanged { get; set; }

    [JsonProperty("lat")] public double Lat { get; set; }
    [JsonProperty("isPoiLocationChanged")] public bool IsPoiLocationChanged { get; set; }
    [JsonProperty("lon")] public double Lon { get; set; }

    [JsonProperty("surveyDuration")] public int SurveyDuration { get; set; }
    [JsonProperty("poiExist")] public bool PoiExist { get; set; }

    [JsonProperty("photoDown")] public PhotoInfo PhotoDown { get; set; }
    [JsonProperty("photoUp")] public PhotoInfo PhotoUp { get; set; }
    [JsonProperty("device")] public DeviceInfo Device { get; set; }


    [JsonConstructor]
    [UnityEngine.Scripting.Preserve]
    public SurveyStoreRequest(string poiId, string placeProposalId,
        string name, bool nameChanged, string category, bool categoryChanged,
        double lat, double lon, bool locationChanged, int duration, bool poiExists)
    {
        PoiId = poiId;
        PlaceProposalId = placeProposalId;

        PoiName = name;
        IsPoiNameChanged = nameChanged;
        PoiCategory = category;
        IsPoiCategoryChanged = categoryChanged;

        Lat = lat;
        Lon = lon;
        IsPoiLocationChanged = locationChanged;
        SurveyDuration = duration;

        PoiExist = poiExists;
        PhotoDown = new PhotoInfo();
        PhotoUp = new PhotoInfo();
        Device = new DeviceInfo { DeviceModel = "unknown", OsVersion = "unknown", CameraDetails = "unknown"};
    }
}
