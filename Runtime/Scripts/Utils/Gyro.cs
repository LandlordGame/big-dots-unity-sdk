using UnityEngine;

namespace Utils
{
    public static class Gyro
	{
		public static void EnableGyro()
		{
			if (SystemInfo.supportsGyroscope) Input.gyro.enabled = true;
		}

		public static GyroInfo GetGyroData()
		{
			if (Input.gyro.enabled == false) return new GyroInfo();

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

