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
			return new GyroInfo
			{
				GyroX = Input.gyro.attitude.x,
                GyroY = Input.gyro.attitude.y,
                GyroZ = Input.gyro.attitude.z,
            };
		}
	}
}

