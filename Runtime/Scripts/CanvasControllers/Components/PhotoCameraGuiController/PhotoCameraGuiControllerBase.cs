using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.CanvasControllers
{
    public abstract class PhotoCameraGuiControllerBase : MonoBehaviour, PhotoCameraGuiController
    {
        public abstract void OpenPhotoCamera(int index);

        public abstract bool AddPhoto(int index, Texture2D photo);
        public abstract bool DoesPhotoExist(int index);
        public abstract bool RemovePhoto(int index);
        public abstract void RemovePhotos();
        public abstract int GetAddedPhotoCount();

        public abstract IList<byte[]> TrimAndEncodeTextureArray();
    }
}
