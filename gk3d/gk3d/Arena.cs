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
        private readonly BasicEffect _effect;
        private readonly GraphicsDevice _graphicsDevice;

        public Arena(ContentManager content, GraphicsDevice device, Vector3 center, int width, int height, int depth, Color color)
            : base(center, width, height, depth, true, color)
        {
            _graphicsDevice = device;
            _effect = new BasicEffect(_graphicsDevice);
            LeftPost = new Cuboid(center + new Vector3(-width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false , Color.SandyBrown);
            RightPost = new Cuboid(center + new Vector3(width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false, Color.SandyBrown);
            Net = new Cuboid(center + new Vector3(0, -height / 8f, 0), (int) (RightPost.Center.X - LeftPost.Center.X), height/4, 1, false, Color.DarkGray);
            _bench = new CustomModel("bench", content, device);
            _android = new CustomModel("android", content, device);
        }

        public void Draw(Camera camera)
        {
            DrawArena(_graphicsDevice, camera);
            DrawLeftPost(_graphicsDevice, camera);
            DrawRightPost(_graphicsDevice, camera);
            DrawNet(_graphicsDevice, camera);
            DrawBenches(camera);
            DrawAndroid(camera);
        }

        private void DrawAndroid(Camera camera)
        {
            _android.Draw(camera, 0.005f, 0, new Vector3(0, -Height / 2.3f, Depth / 3f));
        }

        private void DrawArena(GraphicsDevice device, Camera camera)
        {
            _effect.View = camera.ViewMatrix;
            _effect.Projection = camera.ProjectionMatrix;
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length,
                    Indices, 0, Indices.Length / 3, VertexPositionColor.VertexDeclaration);
            }
        }

        private void DrawLeftPost(GraphicsDevice device, Camera camera)
        {
            _effect.View = camera.ViewMatrix;
            _effect.Projection = camera.ProjectionMatrix;
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, LeftPost.Vertices, 0, LeftPost.Vertices.Length,
                    LeftPost.Indices, 0, LeftPost.Indices.Length / 3, VertexPositionColor.VertexDeclaration);
            }
        }

        private void DrawRightPost(GraphicsDevice device, Camera camera)
        {
            _effect.View = camera.ViewMatrix;
            _effect.Projection = camera.ProjectionMatrix;
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, RightPost.Vertices, 0,
                    RightPost.Vertices.Length,
                    RightPost.Indices, 0, RightPost.Indices.Length/3, VertexPositionColor.VertexDeclaration);
            }
        }

        private void DrawNet(GraphicsDevice device, Camera camera)
        {
            _effect.View = camera.ViewMatrix;
            _effect.Projection = camera.ProjectionMatrix;
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Net.Vertices, 0, Net.Vertices.Length,
                    Net.Indices, 0, Net.Indices.Length / 3, VertexPositionColor.VertexDeclaration);
            }
        }

        private void DrawBenches(Camera camera)
        {
            _bench.Draw(camera, 0.1f, MathHelper.PiOver2, new Vector3(-Width / 3f, -Height / 2f, -Depth / 3f));
            _bench.Draw(camera, 0.1f, MathHelper.PiOver2, new Vector3(Width / 3f, -Height / 2f, Depth / 3f));
        }
    }
}