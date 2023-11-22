using SurveyAPI.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public interface RequestHandlerForPlaceProposal
    {
        public void PostPlaceProposal(double userLat, double userLon, PlaceProposalStoreReqest storeData, IList<byte[]> photos);
    }
}