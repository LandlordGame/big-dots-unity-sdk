using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurveyAPI.Map
{ 
    public class CameraControllerDefault : CameraControllerBase
    {
        [SerializeField] private Camera mapCamera;

        private bool cameraLock = false;


        public override void LockCameraOnPosition(Vector3 position)
        {
            cameraLock = true;

            MoveCameraToPositionIgnoringLock(position);
        }
        public override void UnlockCameraPosition()
        {
            cameraLock = false;
        }
        public override bool IsCameraLocked()
        {
            return cameraLock;
        }

        public override bool MoveCameraToPosition(Vector3 position)
        {
            if (cameraLock == false)
                MoveCameraToPositionIgnoringLock(position);

            return !cameraLock;
        }
        private void MoveCameraToPositionIgnoringLock(Vector3 position)
        {
            mapCamera.transform.position = position;
        }
        public override Vector3 GetCameraPosition()
        {
            return mapCamera.transform.position;
        }
    }
}