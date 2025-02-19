using System.Numerics;

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
    }
}
