using SurveyAPI.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CameraController
{
    public void LockCameraOnPosition(Vector3 position);
    public void UnlockCameraPosition();
    public bool IsCameraLocked();

    public bool MoveCameraToPosition(Vector3 position);
    public Vector3 GetCameraPosition();
}
