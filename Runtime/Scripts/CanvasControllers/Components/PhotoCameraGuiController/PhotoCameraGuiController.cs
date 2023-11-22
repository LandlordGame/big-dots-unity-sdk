using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SurveyAPI.CanvasControllers
{
    public interface PhotoCameraGuiController
    {
        public void OpenPhotoCamera(int index);

        public bool AddPhoto(int index, Texture2D photo);
        public bool DoesPhotoExist(int index);
        public bool RemovePhoto(int index);
        public void RemovePhotos();
        public int GetAddedPhotoCount();

        public IList<byte[]> TrimAndEncodeTextureArray();
    }
}