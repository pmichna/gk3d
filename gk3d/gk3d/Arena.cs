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
        private readonly Effect _effect;

        public Arena(ContentManager content, GraphicsDevice device, Vector3 center, int width, int height, int depth, Color color)
            : base(center, width, height, depth, true, color)
        {
            _graphicsDevice = device;
            LeftPost = new Cuboid(center + new Vector3(-width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false , Color.SandyBrown);
            RightPost = new Cuboid(center + new Vector3(width * 0.2f, -height / 4f, 0), 5, height / 2, 2, false, Color.SandyBrown);
            Net = new Cuboid(center + new Vector3(0, -height / 8f, 0), (int) (RightPost.Center.X - LeftPost.Center.X), height/4, 1, false, Color.DarkGray);
            _effect = content.Load<Effect>("MultipleLightsEffect");
            var lightDirection = new Vector3[2];
            lightDirection[0] = Vector3.Down;
            lightDirection[1] = Vector3.Down;
            var lightPosition = new[] {
                 new Vector3(0f, height/2f - 1, depth/4f),
                 new Vector3(0f, height/2f - 1, -depth/4f)
            };
            _effect.Parameters["LightPosition"].SetValue(lightPosition);
            _effect.Parameters["LightDirection"].SetValue(lightDirection);
            SetModels(content, device);
        }

        Vector3 pointLightColor = new Vector3(0.2f, 0.1f, 1);
        Vector3 pointLightSpecularColor = new Vector3(0.2f, 0.1f, 1);

        public void Draw(Camera camera, double time)
        {
            _effect.Parameters["World"].SetValue(Matrix.Identity);
            _effect.Parameters["View"].SetValue(camera.ViewMatrix);
            _effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            _effect.Parameters["CameraPosition"].SetValue(camera.CameraPosition);
            
            if ((int) time%2 == 1)
            {
                pointLightColor += new Vector3(0.2f, 0.1f, 0);
                pointLightSpecularColor += new Vector3(0, 0.1f, 0);
                _effect.Parameters["PointLightColor"].SetValue(pointLightColor);
                _effect.Parameters["PointLightSpecularColor"].SetValue(pointLightSpecularColor);
            }
            else
            {
                pointLightColor -= new Vector3(0.2f, 0.1f, 0);
                pointLightSpecularColor -= new Vector3(0, 0.1f, 0);
                _effect.Parameters["PointLightColor"].SetValue(pointLightColor);
                _effect.Parameters["PointLightSpecularColor"].SetValue(pointLightSpecularColor);
            }
            DrawArena();
            DrawLeftPost();
            DrawRightPost();
            DrawNet();
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
            foreach (var model in _models)
            {
                model.SetModelEffect(_effect, true);
            }
        }

        private void DrawNet()
        {
            _effect.Parameters["DiffuseColor"].SetValue(Net.Color.ToVector3());
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Net.Vertices, 0, Net.Vertices.Length,
                Net.Indices, 0, Net.Indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
            }
        }

        private void DrawRightPost()
        {
            _effect.Parameters["DiffuseColor"].SetValue(RightPost.Color.ToVector3());
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, RightPost.Vertices, 0,
                    RightPost.Vertices.Length,
                    RightPost.Indices, 0, RightPost.Indices.Length/3, VertexPositionColorNormal.VertexDeclaration);
            }
        }

        private void DrawLeftPost()
        {
            _effect.Parameters["DiffuseColor"].SetValue(LeftPost.Color.ToVector3());
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, LeftPost.Vertices, 0,
                    LeftPost.Vertices.Length,
                    LeftPost.Indices, 0, LeftPost.Indices.Length/3, VertexPositionColorNormal.VertexDeclaration);
            }
        }

        private void DrawArena()
        {
            _effect.Parameters["DiffuseColor"].SetValue(Color.ToVector3());
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length,
                    Indices, 0, Indices.Length/3, VertexPositionColorNormal.VertexDeclaration);
            }
        }
    }
}