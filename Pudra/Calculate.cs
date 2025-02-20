﻿using System.Numerics;

namespace Pudra
{
    public static class Calculate
    {
        public static Vector2 worldToScreen(float[] matrix, Vector3 pos, Vector2 windowSize)
        {
            float screenW = (matrix[12] * pos.X) + (matrix[13] * pos.Y) + (matrix[14] * pos.Z) + matrix[15];

            if (screenW < 0.001f)
            {
                return new Vector2(-1, -1);
            }

            float screenX = (matrix[0] * pos.X) + (matrix[1] * pos.Y) + (matrix[2] * pos.Z) + matrix[3];
            float screenY = (matrix[4] * pos.X) + (matrix[5] * pos.Y) + (matrix[6] * pos.Z) + matrix[7];

            float X = (windowSize.X / 2) * (1 + screenX / screenW);
            float Y = (windowSize.Y / 2) * (1 - screenY / screenW);

            return new Vector2(X, Y);
        }

        public static Vector2 calculateAngles(Vector3 from, Vector3 to)
        {
            float yaw;
            float pitch;

            float deltaX = to.X - from.X;
            float deltaY = to.Y - from.Y;
            float deltaZ = to.Z - from.Z;
            double distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            yaw = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI);
            pitch = -(float)(Math.Atan2(deltaZ, distance) * 180 / Math.PI);

            return new Vector2(yaw, pitch);
        }
    }
}
