using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class PhotoCameraFake : PhotoCameraBase
{
    [Header("Canvas")]
    [SerializeField] GameObject container;
    [SerializeField] TextMeshProUGUI loadingLabel;
    [SerializeField] RawImage rawImage;

    [Header("Config")]
    [SerializeField] Texture2D fakePhoto;


    private Action<Texture2D> tookPhotoAction;
    private Action closeAction;



    private void Update()
    {
        rawImage.texture = fakePhoto;
    }


    public override void Show(bool frontCamera, bool closeAfterTakingPhoto, Action closeAction, Action<Texture2D> tookPhotoAction)
    {
        container.SetActive(true);

        this.closeAction = closeAction;
        this.tookPhotoAction = tookPhotoAction;

        loadingLabel.gameObject.SetActive(false);
    }
    public override void Hide()
    {
        container.SetActive(false);
    }

    public void HandleTakePhotoButton()
    {
        if (tookPhotoAction == null)
            return;

        Hide();

        Texture2D newTexture = PrepareTextureToSend(fakePhoto);
        tookPhotoAction.Invoke(newTexture);
    }
    private Texture2D PrepareTextureToSend(Texture2D sourceTexture)
    {
        Texture2D textureToSend = new Texture2D(sourceTexture.width,sourceTexture.height,TextureFormat.RGB24,false);
        textureToSend.SetPixels32(sourceTexture.GetPixels32());
        textureToSend.Apply();

        return textureToSend;
    }

    public void HandleCloseButton()
    {
        Hide();

        closeAction?.Invoke();
    }
}
