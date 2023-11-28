using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class PhotoCameraBasic : PhotoCameraBase
{
    [Header("Canvas")]
    [SerializeField] GameObject container;
    [SerializeField] TextMeshProUGUI loadingLabel;
    [SerializeField] RawImage rawImage;
    [SerializeField] Vector2Int[] preferredResolutions; 


    private bool closeAfterTakingPhoto = true;
    private Action closeAction;
    private Action<Texture2D, Utils.GyroInfo> tookPhotoAction;
    private WebCamTexture webcamTexture;
    private Texture2D temporaryTexture;
    private Color32[] arrayForTextureManipulation;


    private void Awake()
    {
        if (preferredResolutions.Length == 0)
            preferredResolutions = new Vector2Int[] { new Vector2Int(1280,720) };
    }

    void Update()
    {
        if (webcamTexture == false)
            return;

        if (temporaryTexture != null)
            Destroy(temporaryTexture);

        if (webcamTexture.videoRotationAngle == 90)
            temporaryTexture = ConvertWebcamTexure(true,true); else
            temporaryTexture = ConvertWebcamTexure(false,true);

        rawImage.texture = temporaryTexture;
    }

    public override void Show(bool useFrontCameraOnMobile, bool closeAfterTakingPhoto,
        Action closeAction, Action<Texture2D, Utils.GyroInfo> tookPhotoAction)
    {
        this.closeAfterTakingPhoto = closeAfterTakingPhoto;
        this.closeAction = closeAction;
        this.tookPhotoAction = tookPhotoAction;

        void permissionGranted(string _)
        {
            container.SetActive(true);
            ShowLoadingLabel();

            InitPhotoCamera(useFrontCameraOnMobile);
        }
        void permissionDenied(string _)
        {
            container.SetActive(true);
            ShowLoadingLabel("PhotoCameraBasic has no permission - check app settings");
        };

        AskForPermission(permissionGranted,permissionDenied);
    }

    public override void Hide()
    {
        if (webcamTexture != null)
            webcamTexture.Stop();

        container.SetActive(false);
    }
    private void ShowLoadingLabel(string text = "PhotoCameraBasic is loading")
    {
        loadingLabel.gameObject.SetActive(true);
        SetLoadingLabelText(text);
    }
    private void HideLoadingLabel()
    {
        loadingLabel.gameObject.SetActive(false);
    }
    private void SetLoadingLabelText(string text)
    {
        if (text != null)
            loadingLabel.text = text;
    }

    private void AskForPermission(Action<string> permissionGranted, Action<string> permissionDenied)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            PermissionCallbacks callbacks = new PermissionCallbacks();
            if (permissionGranted != null)
                callbacks.PermissionGranted += permissionGranted;
            if (permissionDenied != null)
            {
                callbacks.PermissionDenied += permissionDenied;
                callbacks.PermissionDeniedAndDontAskAgain += permissionDenied;
            }

            Permission.RequestUserPermission(Permission.Camera,callbacks);
        } else
        {
            permissionGranted?.Invoke("OK");
        }
    }

    private void InitPhotoCamera(bool useFrontCameraOnMobile)
    {
        try
        {
            webcamTexture = FindWebCam(useFrontCameraOnMobile);
            webcamTexture.Play();

            HideLoadingLabel();

        } catch (Exception e)
        {
            Debug.LogError(e.ToString());

            webcamTexture = null;
            SetLoadingLabelText("PhotoCameraBasic:\nDevice not ready, try again");
        }
    }
    private WebCamTexture FindWebCam(bool useFrontCameraOnMobile)
    {
        WebCamTexture result;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Resolution closestResolution = FindClosestResolutionOnMobile(useFrontCameraOnMobile);
            result = new WebCamTexture(closestResolution.height,closestResolution.width);
        } else
        {
            result = new WebCamTexture(preferredResolutions[0].x,preferredResolutions[0].y);
        }

        if (result == null)
            throw new Exception("PhotoCameraBasic - no device or proper resolution found.");

        return result;
    }
    private Resolution FindClosestResolutionOnMobile(bool useFrontCameraOnMobile)
    {
        Resolution? closestResolution = null;
        int preferredPixelCount = preferredResolutions[0].x * preferredResolutions[0].y;
        int closestPixelCount = 0;

        foreach (WebCamDevice device in WebCamTexture.devices)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                if (device.isFrontFacing != useFrontCameraOnMobile)
                    continue;

            Resolution[] deviceResolutions = device.availableResolutions;
            foreach (Resolution deviceResolution in deviceResolutions)
            {
                foreach (Vector2Int preferredResolution in preferredResolutions)
                    if (preferredResolution.x == deviceResolution.width && preferredResolution.y == deviceResolution.height)
                        return deviceResolution;

                int deviceResolutionPixelCount = deviceResolution.width * deviceResolution.height;
                if (closestResolution.HasValue == false || Math.Abs(preferredPixelCount - deviceResolutionPixelCount) <= closestPixelCount)
                {
                    closestPixelCount = deviceResolutionPixelCount;
                    closestResolution = deviceResolution;
                }
            }
        }

        if (closestResolution.HasValue == true)
            return closestResolution.Value;

        throw new Exception("PhotoCameraBasic - no proper resolution found.");
    }

    private Texture2D ConvertWebcamTexure(bool rotate90deg, bool mirrored)
    {
        if (webcamTexture.isPlaying == false)
            return new Texture2D(16, 16, TextureFormat.RGB24, false);

        Color32[] data = webcamTexture.GetPixels32();
        if (arrayForTextureManipulation == null || arrayForTextureManipulation.Length < data.Length)
            arrayForTextureManipulation = new Color32[data.Length];

        int width = webcamTexture.width;
        int height = webcamTexture.height;

        int size = width * height;

        if (rotate90deg == true)
        {
            for (int a = 0; a < height; a++)
                for (int b = 0; b < width; b++)
                    arrayForTextureManipulation[size - height * (b + 1) + a] = data[size - width * (a + 1) + b];

            int tempWidth = width;
            width = height;
            height = tempWidth;
        }
        else
        {
            System.Array.Copy(data, arrayForTextureManipulation, data.Length);
        }

        if (mirrored == true)
            for (int a = 0; a < height; a++)
                System.Array.Reverse(arrayForTextureManipulation, a * width, width);

        Texture2D result = new Texture2D(width, height, TextureFormat.RGB24, false);

        if (width * height == arrayForTextureManipulation.Length)
        {
            result.SetPixels32(arrayForTextureManipulation, 0);
            result.Apply();
        }

        return result;
    }


    public void HandleCloseButton()
    {
        Hide();

        closeAction?.Invoke();
    }
    public void HandleTakePhotoButton()
    {
        if (tookPhotoAction == null || webcamTexture == null)
            return;

        Texture2D textureToSend = PrepareTextureToSend(temporaryTexture);
        Utils.Gyro.EnableGyro();
        var gyroInfo = Utils.Gyro.GetGyroData();
        tookPhotoAction.Invoke(textureToSend, gyroInfo);

        if (closeAfterTakingPhoto == true)
            Hide();
    }
    private Texture2D PrepareTextureToSend(Texture2D sourceTexture)
    {
        Texture2D textureToSend = new Texture2D(sourceTexture.width,sourceTexture.height,TextureFormat.RGB24,false);
        textureToSend.SetPixels32(sourceTexture.GetPixels32());
        textureToSend.Apply();

        return textureToSend;
    }
}
