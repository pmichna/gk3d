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
            GenerateTags();
            Position = position;
            Rotation = rotation;
            Scale = scale;
            _graphicsDevice = graphicsDevice;
        }

        public void Draw(Matrix view, Matrix projection, Vector3 CameraPosition)
        {
            var baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
            foreach (var mesh in Model.Meshes)
            {
                var localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;
                foreach (var meshPart in mesh.MeshParts)
                {
                    var effect = meshPart.Effect;
                    if (effect is BasicEffect)
                    {
                        ((BasicEffect) effect).World = localWorld;
                        ((BasicEffect) effect).View = view;
                        ((BasicEffect) effect).Projection = projection;
                        ((BasicEffect) effect).EnableDefaultLighting();
                    }
                    else
                    {
                        SetEffectParameter(effect, "World", localWorld);
                        SetEffectParameter(effect, "View", view);
                        SetEffectParameter(effect, "Projection", projection);
                        SetEffectParameter(effect, "CameraPosition", CameraPosition);
                    }
                }
                mesh.Draw();
            }
        }

        // Store references to all of the model's current effects
        public void CacheEffects()
        {
            foreach (var mesh in Model.Meshes)
                foreach (var part in mesh.MeshParts)
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }

        // Restore the effects referenced by the model's cache
        public void RestoreEffects()
        {
            foreach (var mesh in Model.Meshes)
                foreach (var part in mesh.MeshParts)
                    if (((MeshTag)part.Tag).CachedEffect != null)
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
        }

        public void SetModelEffect(Effect effect, bool copyEffect)
        {
            foreach (var mesh in Model.Meshes)
                foreach (var part in mesh.MeshParts)
                {
                    var toSet = effect;
                    // Copy the effect if necessary
                    if (copyEffect)
                        toSet = effect.Clone();
                    var tag = ((MeshTag)part.Tag);
                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        SetEffectParameter(toSet, "BasicTexture", tag.Texture);
                        SetEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                        SetEffectParameter(toSet, "TextureEnabled", false);
                    // Set our remaining parameters to the effect
                    SetEffectParameter(toSet, "DiffuseColor", tag.Color);
                    SetEffectParameter(toSet, "SpecularPower", tag.SpecularPower);
                    part.Effect = toSet;
                }
        }

        private static void SetEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null) return;
            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
        }

        private void GenerateTags()
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    if (part.Effect is BasicEffect)
                    {
                        var effect = (BasicEffect) part.Effect;
                        var tag = new MeshTag(effect.DiffuseColor, effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }
                }
            }
        }
    }
}