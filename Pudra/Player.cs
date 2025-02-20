using System.Numerics;

namespace Pudra
{
    public class Player
    {
        public Vector3 position { get; set; }
        public Vector3 viewOffset { get; set; }
        public Vector2 position2D { get; set; }
        public Vector2 viewPosition2D { get; set; }
        public uint health { get; set; }
        public string name { get; set; }
        public uint lifeState { get; set; }
        public int team { get; set; }
        public IntPtr pawnAddress { get; set; }
    }
}
