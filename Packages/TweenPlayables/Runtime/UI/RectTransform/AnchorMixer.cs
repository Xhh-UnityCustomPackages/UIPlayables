using System;
using UnityEngine;

namespace TweenPlayables
{
    #region Anchor
    
    public enum AnchorPreset
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        Custom,
    }

    public class AnchorMixer : ValueMixer<Vector4>
    {
        protected override void BlendCore(Vector4 value, float weight)
        {
            Value += value * weight;
        }

        private Vector4 GetAnchorVector(AnchorPreset preset)
        {
            switch (preset)
            {
                // 顶部锚点
                case AnchorPreset.TopLeft: return new Vector4(0, 1, 0, 1);
                case AnchorPreset.TopCenter: return new Vector4(0.5f, 1, 0.5f, 1);
                case AnchorPreset.TopRight: return new Vector4(1, 1, 1, 1);
                // 中部锚点
                case AnchorPreset.MiddleLeft: return new Vector4(0, 0.5f, 0, 0.5f);
                case AnchorPreset.MiddleCenter: return new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
                case AnchorPreset.MiddleRight: return new Vector4(1, 0.5f, 1, 0.5f);
                // 底部锚点
                case AnchorPreset.BottomLeft: return new Vector4(0, 0, 0, 0);
                case AnchorPreset.BottomCenter: return new Vector4(0.5f, 0, 0.5f, 0);
                case AnchorPreset.BottomRight: return new Vector4(1, 0, 1, 0);
            }

            return new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
    
    
    [Serializable]
    public sealed class AnchorTweenParameter : TweenParameter<Vector4>
    {
        public override Vector4 GetRelativeValue(object key, Vector4 value)
        {
            return GetInitialValue(key) + value;
        }

        public override Vector4 Evaluate(object key, float t)
        {
            if (IsKeyframe) return EndValue;
            if (IsRelative) return Vector4.LerpUnclamped(GetRelativeValue(key, StartValue), GetRelativeValue(key, EndValue), EaseParameter.Evaluate(t));
            else return Vector4.LerpUnclamped(StartValue, EndValue, EaseParameter.Evaluate(t));
        }
    }
    #endregion

}