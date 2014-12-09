using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace gk3d
{
    class Arena : Cuboid
    {
        private Cuboid LeftPost { get; set; }
        private Cuboid RightPost { get; set; }
        private Cuboid Net { get; set; }
        private readonly CustomModel _bench;
        private readonly CustomModel _android;
        private readonly Effect _effect;
        private readonly GraphicsDevice _graphicsDevice;

        public Arena(ContentManager content, GraphicsDevice device, Vector3 center, int width, int height, int depth, Color color)
            : base(center, width, height, depth, true, color)
        {
            _graphicsDevice = device;
            _effect = content.Load<Effect>("effect");
            LeftPost = new Cuboid(center + new Vector3(-width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false , Color.SandyBrown);
            RightPost = new Cuboid(center + new Vector3(width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false, Color.SandyBrown);
            Net = new Cuboid(center + new Vector3(0, -height / 8f, 0), (int) (RightPost.Center.X - LeftPost.Center.X), height/4, 1, false, Color.DarkGray);
            _effect.CurrentTechnique = _effect.Techniques["SpotLight"];
            _effect.Parameters["xLightPosition"].SetValue(center + new Vector3(0, height/2f, depth/4f));
            _effect.Parameters["xAmbient"].SetValue(0.1f);
            _effect.Parameters["xConeDirection"].SetValue(new Vector3(0, -1, 0));
            _effect.Parameters["xConeAngle"].SetValue(0.3f);
            _effect.Parameters["xConeDecay"].SetValue(2f);
            _effect.Parameters["xLightStrength"].SetValue(1.5f);
            _bench = new CustomModel(_effect, "bench", content, device);
            _android = new CustomModel(_effect, "android", content, device);
        }

        public void Draw(Camera camera)
        {
            _effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            _effect.Parameters["xView"].SetValue(camera.ViewMatrix);
            _effect.Parameters["xProjection"].SetValue(camera.ProjectionMatrix);
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawArena();
                DrawLeftPost();
                DrawRightPost();
                DrawNet();
            }
            DrawBenches(camera);
            DrawAndroid(camera);
        }

        private void DrawNet()
        {
            _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Net.Vertices, 0, Net.Vertices.Length,
                Net.Indices, 0, Net.Indices.Length/3, VertexPositionColorNormal.VertexDeclaration);
        }

        private void DrawRightPost()
        {
            _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, RightPost.Vertices, 0,
                RightPost.Vertices.Length,
                RightPost.Indices, 0, RightPost.Indices.Length/3, VertexPositionColorNormal.VertexDeclaration);
        }

        private void DrawLeftPost()
        {
            _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, LeftPost.Vertices, 0, LeftPost.Vertices.Length,
                LeftPost.Indices, 0, LeftPost.Indices.Length/3, VertexPositionColorNormal.VertexDeclaration);
        }

        private void DrawArena()
        {
            _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length,
                Indices, 0, Indices.Length/3, VertexPositionColorNormal.VertexDeclaration);
        }

        private void DrawAndroid(Camera camera)
        {
            _android.Draw(camera, 0.005f, MathHelper.Pi, new Vector3(0, -Height / 2.3f, Depth / 3f));
        }

        private void DrawBenches(Camera camera)
        {
            _bench.Draw(camera, 0.1f, MathHelper.PiOver2, new Vector3(-Width / 3f, -Height / 2f, -Depth / 3f));
            _bench.Draw(camera, 0.1f, MathHelper.PiOver2, new Vector3(Width / 3f, -Height / 2f, Depth / 3f));
        }
    }
}