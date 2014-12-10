using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gk3d
{
    public class CModel
    {
        private Vector3 Position { get; set; }
        private Vector3 Rotation { get; set; }
        private Vector3 Scale { get; set; }
        private Model Model { get; set; }
        private readonly Matrix[] modelTransforms;
        private GraphicsDevice _graphicsDevice;

        public CModel(Model model, Vector3 position, Vector3 rotation, Vector3 scale, GraphicsDevice graphicsDevice)
        {
            Model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Position = position;
            Rotation = rotation;
            Scale = scale;
            _graphicsDevice = graphicsDevice;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Calculate the base transformation by combining
            // translation, rotation, and scaling
            var baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
            foreach (var mesh in Model.Meshes)
            {
                var localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;
                foreach (var meshPart in mesh.MeshParts)
                {
                    var effect = (BasicEffect)meshPart.Effect;
                    effect.World = localWorld;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}