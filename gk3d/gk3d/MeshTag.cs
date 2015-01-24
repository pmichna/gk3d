using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gk3d
{
    class MeshTag
    {
        public Vector4 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;

        public MeshTag(Vector4 color, Texture2D texture, float specularPower)
        {
            Color = color;
            Texture = texture;
            SpecularPower = specularPower;
        }
    }
}
