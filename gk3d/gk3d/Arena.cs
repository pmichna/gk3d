using Microsoft.Xna.Framework;

namespace gk3d
{
    class Arena : Cuboid
    {
        public Arena(Vector3 center, int width, int height, int depth, Color color)
            : base(center, width, height, depth, true, color)
        {
        }
    }
}
