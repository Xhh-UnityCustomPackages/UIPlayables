using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TweenPlayables
{
    public class TweenRectMask2DMixerBehaviour : TweenAnimationMixerBehaviour<RectMask2D, TweenRectMask2DBehaviour>
    {
        readonly Vector4ValueMixer paddingMixer = new();
        
        public override void Blend(RectMask2D binding, TweenRectMask2DBehaviour behaviour, float weight, float progress)
        {
            paddingMixer.TryBlend(behaviour.Padding, binding, progress, weight);
        }

        public override void Apply(RectMask2D binding)
        {
            paddingMixer.TryApplyAndClear(binding, (x, binding) => binding.padding = x);
        }
    }
}
