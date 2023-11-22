using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Service
{
    public class SurveyServiceFileContent
    {
        public byte[] Data { get; private set; }
        public string ContentName { get; private set; }
        public string FileName { get; private set; }

        public SurveyServiceFileContent(byte[] data, string contentName, string fileName)
        {
            this.Data = data;
            this.ContentName = contentName;
            this.FileName = fileName;
        }
    }
}
