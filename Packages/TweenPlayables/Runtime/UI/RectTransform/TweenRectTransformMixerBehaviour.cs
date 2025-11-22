using UnityEngine;

namespace TweenPlayables
{
    public class TweenRectTransformMixerBehaviour : TweenAnimationMixerBehaviour<RectTransform, TweenRectTransformBehaviour>
    {
        readonly Vector3ValueMixer anchoredPositionMixer = new();
        readonly Vector2ValueMixer sizeDeltaMixer = new();
        readonly Vector3ValueMixer rotationMixer = new();
        readonly Vector3ValueMixer scaleMixer = new();
        readonly AnchorMixer anchorMixer = new();
        readonly Vector2ValueMixer pivotMixer = new();

        public override void Blend(RectTransform binding, TweenRectTransformBehaviour behaviour, float weight, float progress)
        {
            anchoredPositionMixer.TryBlend(behaviour.AnchoredPosition, binding, progress, weight);
            sizeDeltaMixer.TryBlend(behaviour.SizeDelta, binding, progress, weight);
            rotationMixer.TryBlend(behaviour.Rotation, binding, progress, weight);
            scaleMixer.TryBlend(behaviour.Scale, binding, progress, weight);
            anchorMixer.TryBlend(behaviour.Anchor, binding, progress, weight);
            pivotMixer.TryBlend(behaviour.Pivot, binding, progress, weight);
        }

        public override void Apply(RectTransform binding)
        {
            anchoredPositionMixer.TryApplyAndClear(binding, (x, binding) => binding.anchoredPosition3D = x);
            sizeDeltaMixer.TryApplyAndClear(binding, (x, binding) => binding.sizeDelta = x);
            rotationMixer.TryApplyAndClear(binding, (x, binding) => binding.localEulerAngles = x);
            scaleMixer.TryApplyAndClear(binding, (x, binding) => binding.localScale = x);
            anchorMixer.TryApplyAndClear(binding, (x, binding) =>
            {
                binding.anchorMin = new Vector2(x.x, x.y);
                binding.anchorMax = new Vector2(x.z, x.w);
            });
            pivotMixer.TryApplyAndClear(binding, (x, binding) => binding.pivot = x);
        }
    }
}
