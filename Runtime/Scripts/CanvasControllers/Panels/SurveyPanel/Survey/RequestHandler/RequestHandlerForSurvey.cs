using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public interface RequestHandlerForSurvey
    {
        public void PostSurvey(double userLat, double userLon, SurveyStoreRequest storeData, IList<byte[]> photos);
    }
}