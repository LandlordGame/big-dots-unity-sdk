using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraControllerBase : MonoBehaviour, CameraController
{
    public abstract void LockCameraOnPosition(Vector3 position);
    public abstract void UnlockCameraPosition();
    public abstract bool IsCameraLocked();

    public abstract bool MoveCameraToPosition(Vector3 position);
    public abstract Vector3 GetCameraPosition();
}
