using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace gk3d
{
    class CustomModel
    {
        private readonly Model _model;

        public CustomModel(Effect effect, string assetName, ContentManager content, GraphicsDevice device)
        {
            _model = content.Load<Model>(assetName);
            foreach (var mesh in _model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();
        }

        public void Draw(Camera camera, float scale, float rotationY, Vector3 translation)
        {
            var worldMatrix = Matrix.CreateScale(scale, scale, scale) * Matrix.CreateRotationY(rotationY) * Matrix.CreateTranslation(translation);
            var modelTransforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = modelTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
