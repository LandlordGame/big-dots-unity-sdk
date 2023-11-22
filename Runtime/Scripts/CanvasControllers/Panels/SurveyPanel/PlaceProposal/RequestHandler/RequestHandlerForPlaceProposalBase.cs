using SurveyAPI.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public abstract class RequestHandlerForPlaceProposalBase : MonoBehaviour, RequestHandlerForPlaceProposal
    {
        public abstract void PostPlaceProposal(double userLat, double userLon, PlaceProposalStoreReqest storeData, IList<byte[]> photos);
    }
}
