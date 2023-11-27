using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhotoCameraBase : MonoBehaviour, PhotoCamera
{
    public abstract void Show(bool useFrontCameraOnMobile, bool closeAfterTakingPhoto, Action closeAction, Action<Texture2D, Utils.GyroInfo> tookPhotoAction);
    public abstract void Hide();
}
