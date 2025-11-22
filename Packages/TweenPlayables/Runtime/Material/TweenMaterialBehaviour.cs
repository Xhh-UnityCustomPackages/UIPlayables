using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

namespace TweenPlayables
{
    [Serializable]
    public sealed class TweenMaterialBehaviour : PlayableBehaviour
    {
        public string propertyName = "";
        public ShaderPropertyType propertyType = ShaderPropertyType.Float;
        public Vector4 vector;
        
        
        public TweenMaterialMixerBehaviour mixer;
        
        public TweenMaterialBehaviour() : base()
        {
        }
        
        public TweenMaterialBehaviour(TweenMaterialBehaviour other)
        {
            propertyName = other.propertyName;
            propertyType = other.propertyType;
            vector = other.vector;
            mixer = other.mixer;
        }
        
        #region MaterialPropertyBlock

        public bool HasProperty(MaterialPropertyBlock block)
        {
            return block.HasProperty(propertyName);
        }
        
        public void ApplyFromPropertyBlock(MaterialPropertyBlock source)
        {
            switch (propertyType)
            {
                case ShaderPropertyType.Float:
                case ShaderPropertyType.Range:
                    vector.x = source.GetFloat(propertyName);
                    break;
                case ShaderPropertyType.Texture:
                    vector = source.GetVector(propertyName + "_ST");
                    // vector = source.GetTextureScaleOffset(propertyName);
                    break;
                default:
                    vector = source.GetVector(propertyName);
                    break;
            }
        }
        
        public void ApplyToPropertyBlock(MaterialPropertyBlock target)
        {
            switch (propertyType)
            {
                case ShaderPropertyType.Float:
                case ShaderPropertyType.Range:
                    target.SetFloat(propertyName, vector.x);
                    break;
                case ShaderPropertyType.Texture:
                    target.SetVector(propertyName + "_ST", vector);
                    // target.SetTextureScaleOffset(propertyName, vector);
                    break;
                case ShaderPropertyType.Color:
                    // Use SetColor() to apply gamma correction in HDRP
                    target.SetColor(propertyName, vector);
                    break;
                default:
                    target.SetVector(propertyName, vector);
                    break;
            }
        }

        #endregion

        #region Material
        bool HasProperty(Material material) => material.HasProperty(Shader.PropertyToID(propertyName));
        
        public void ApplyFromMaterial(Material source)
        {
            if (!HasProperty(source))
                return;

            Shader shader = source.shader;
            int propIndex = shader.FindPropertyIndex(propertyName);
            switch (propertyType)
            {
                case ShaderPropertyType.Float:
                    vector.x = source.GetFloat(propertyName);
                    break;
                case ShaderPropertyType.Range:
                    Vector2 limits = shader.GetPropertyRangeLimits(propIndex);

                    // Pack range limits into unused vector components
                    vector.x = source.GetFloat(propertyName);
                    vector.y = limits.x;
                    vector.z = limits.y;
                    break;
                case ShaderPropertyType.Texture:
                    Vector2 scale = source.GetTextureScale(propertyName);
                    Vector2 offset = source.GetTextureOffset(propertyName);
                    vector = new Vector4(scale.x, scale.y, offset.x, offset.y);
                    break;
                default:
                    vector = source.GetVector(propertyName);
                    break;
            }
        }

        public void ApplyToMaterial(Material target)
        {
            if (!HasProperty(target))
                return;

            switch (propertyType)
            {
                case ShaderPropertyType.Float:
                case ShaderPropertyType.Range:
                    target.SetFloat(propertyName, vector.x);
                    break;
                case ShaderPropertyType.Texture:
                    target.SetTextureScale(propertyName, vector);
                    target.SetTextureOffset(propertyName, new Vector2(vector.z, vector.w));
                    break;
                default:
                    target.SetVector(propertyName, vector);
                    break;
            }
        }
        
        #endregion


        public bool IsBlendableWith(TweenMaterialBehaviour other) => other != null
                && propertyName == other.propertyName
                && propertyType == other.propertyType
                && (propertyType != ShaderPropertyType.Texture);
        
        
        public void Lerp(TweenMaterialBehaviour a, TweenMaterialBehaviour b, float t)
        {
            vector = Vector4.Lerp(a.vector, b.vector, t);
        }
    }
}