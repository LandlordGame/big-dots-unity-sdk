using UnityEngine;

namespace Utils
{
    public static class Gyro
	{
		public static void EnableGyro()
		{
			Input.gyro.enabled = true;
		}

		public static GyroInfo GetGyroData()
		{
			var eulerAnglesGyro = Input.gyro.attitude.eulerAngles;

            return new GyroInfo
			{
				GyroX = eulerAnglesGyro.x,
                GyroY = eulerAnglesGyro.y,
                GyroZ = eulerAnglesGyro.z,
            };
		}
	}
}

