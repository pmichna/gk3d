﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gk3d
{
    public class CModel
    {
        private Vector3 Position { get; set; }
        private Vector3 Rotation { get; set; }
        private Vector3 Scale { get; set; }
        private Model Model { get; set; }
        private readonly Matrix[] _modelTransforms;
        //private GraphicsDevice _graphicsDevice;
        
        public CModel(Model model, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Model = model;
            _modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(_modelTransforms);
            GenerateTags();
            Position = position;
            Rotation = rotation;
            Scale = scale;
            //_graphicsDevice = graphicsDevice;
        }

        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition, bool isFogEnabled, float fogPower)
        {
            var baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
            foreach (var mesh in Model.Meshes)
            {
                var localWorld = _modelTransforms[mesh.ParentBone.Index] * baseWorld;
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
                        SetEffectParameter(effect, "CameraPosition", cameraPosition);
                        SetEffectParameter(effect, "FogEnabled", isFogEnabled);
                        SetEffectParameter(effect, "FogPower", fogPower);
                    }
                }
                mesh.Draw();
            }
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
                        SetEffectParameter(toSet, "xTexture", tag.Texture);
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
            else if (val is Vector4)
                effect.Parameters[paramName].SetValue((Vector4)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
            else if (val is float)
                effect.Parameters[paramName].SetValue((float)val);
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
                        var tag = new MeshTag(new Vector4(effect.DiffuseColor, 1), effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }
                }
            }
        }
    }
}