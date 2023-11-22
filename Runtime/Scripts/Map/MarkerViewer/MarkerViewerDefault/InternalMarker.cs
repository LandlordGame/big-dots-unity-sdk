using SurveyAPI.Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SurveyAPI.Map
{
    public class InternalMarker : MonoBehaviour, Marker
    {
        [SerializeField] private TextMeshPro title;
        [SerializeField] private GameObject mainObject;

        public void SetTitle(string text)
        {
            title.text = text;
        }
        public void SetMainObjectScale(float scale)
        {
            mainObject.transform.localScale = new Vector3(scale,scale,scale);
        }

        public Vector3 GetPosition()
        {
            return this.gameObject.transform.position;
        }
    }
}