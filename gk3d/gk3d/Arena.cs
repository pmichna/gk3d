using System.Collections.Generic;
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
        private readonly GraphicsDevice _graphicsDevice;
        private readonly List<CModel> _models = new List<CModel>(3);

        public Arena(ContentManager content, GraphicsDevice device, Vector3 center, int width, int height, int depth, Color color)
            : base(center, width, height, depth, true, color)
        {
            _graphicsDevice = device;
            LeftPost = new Cuboid(center + new Vector3(-width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false , Color.SandyBrown);
            RightPost = new Cuboid(center + new Vector3(width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false, Color.SandyBrown);
            Net = new Cuboid(center + new Vector3(0, -height / 8f, 0), (int) (RightPost.Center.X - LeftPost.Center.X), height/4, 1, false, Color.DarkGray);

            SetModels(content, device);
        }

        public void Draw(Camera camera)
        {
            //_effect.Parameters["World"].SetValue(Matrix.Identity);
            //_effect.Parameters["View"].SetValue(camera.ViewMatrix);
            //_effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            //_effect.Parameters["CameraPosition"].SetValue(camera.CameraPosition);
            //foreach (var pass in _effect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    DrawArena();
            //    DrawLeftPost();
            //    DrawRightPost();
            //    DrawNet();
            //}
            foreach (CModel model in _models)
                model.Draw(camera.ViewMatrix, camera.ProjectionMatrix, camera.CameraPosition);
        }

        private void SetModels(ContentManager content, GraphicsDevice device)
        {
            var benchModel = content.Load<Model>("bench");
            var androidModel = content.Load<Model>("android");

            _models.Add(new CModel(benchModel,
                Center + new Vector3(-Width/3f, -Height/2f, -Depth/3f),
                new Vector3(0, MathHelper.PiOver2, 0),
                new Vector3(0.1f),
                device));
            _models.Add(new CModel(benchModel,
                Center + new Vector3(Width/3f, -Height/2f, Depth/3f),
                new Vector3(0, MathHelper.PiOver2, 0),
                new Vector3(0.1f),
                device));
            _models.Add(new CModel(androidModel,
                Center + new Vector3(0, -Height/2.3f, Depth/3f),
                new Vector3(0, MathHelper.Pi, 0),
                new Vector3(0.005f),
                device));

            var simpleEffect = content.Load<Effect>("SimpleEffect");
            foreach (var model in _models)
            {
                model.SetModelEffect(simpleEffect, true);
            }
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
    }
}