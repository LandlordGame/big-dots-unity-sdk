using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public abstract class RequestHandlerForSurveyBase : MonoBehaviour, RequestHandlerForSurvey
    {
        public abstract void PostSurvey(double userLat, double userLon, SurveyStoreRequest storeData, IList<byte[]> photos);
    }
}
