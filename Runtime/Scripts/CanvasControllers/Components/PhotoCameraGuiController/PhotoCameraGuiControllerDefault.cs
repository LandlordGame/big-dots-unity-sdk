using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurveyAPI.CanvasControllers
{
    public class PhotoCameraGuiControllerDefault : PhotoCameraGuiControllerBase
    {
        public enum ImageType
        {
            JPG,PNG
        }

        [Header("External")]
        [SerializeField] PhotoCameraBase photoCamera;
        [Header("Canvas")]
        [SerializeField] RawImage[] photoImages;
        [Header("Config")]
        [SerializeField] ImageType imageType;

        
        private Texture2D[] texturesForPhotoImages;
        private int addedPhotosCount;

        
        private void Awake()
        {
            texturesForPhotoImages = new Texture2D[photoImages.Length];
        }

        public override void OpenPhotoCamera(int index)
        {
            Action closeAction = () => { };
            Action<Texture2D> tookPhotoAction = (Texture2D photo) => AddPhoto(index,photo);

            photoCamera.Show(false,true,closeAction,tookPhotoAction);
        }
        public override bool AddPhoto(int index, Texture2D photo)
        {
            if (texturesForPhotoImages[index] != null)
                return false;

            photoImages[index].texture = photo;
            photoImages[index].color = new Color(1,1,1,1);

            texturesForPhotoImages[index] = photo;
            addedPhotosCount++;

            return true;
        }
        public override bool RemovePhoto(int index)
        {
            if (DoesPhotoExist(index) == false)
                return false;

            Destroy(texturesForPhotoImages[index]);

            photoImages[index].texture = null;
            photoImages[index].color = new Color(0, 0, 0, 0);
            texturesForPhotoImages[index] = null;
            addedPhotosCount--;

            return true;
        }
        public override void RemovePhotos()
        {
            for (int i =0; i < texturesForPhotoImages.Length; i++)
                RemovePhoto(i);
        }

        public override bool DoesPhotoExist(int index)
        {
            if (texturesForPhotoImages[index] == null)
                return false; else
                return true;
        }
        public override IList<byte[]> TrimAndEncodeTextureArray()
        {
            List<byte[]> photoArray = new List<byte[]>();
            foreach (Texture2D texture in texturesForPhotoImages)
                if (texture != null)
                {
                    if (imageType == ImageType.JPG)
                        photoArray.Add(texture.EncodeToJPG()); else
                        photoArray.Add(texture.EncodeToPNG());
                }

            return photoArray;
        }

        public override int GetAddedPhotoCount()
        {
            return addedPhotosCount;
        }
    }
}