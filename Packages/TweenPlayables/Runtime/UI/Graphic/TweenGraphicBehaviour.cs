using System;
using UnityEngine;
using UnityEngine.UI;

namespace TweenPlayables
{
    [Serializable]
    public sealed class TweenGraphicBehaviour : TweenAnimationBehaviour<Graphic>
    {
        [SerializeField] ColorTweenParameter color;

        public ReadOnlyTweenParameter<Color> Color => color;

        public override void OnTweenInitialize(Graphic playerData)
        {
            color.SetInitialValue(playerData, playerData.color);
        }
    }


    [Serializable]
    public sealed class MaterialTweenParameter : TweenParameter<Color>
    {
        public override Color Evaluate(object key, float t)
        {
            throw new NotImplementedException();
        }

        public override Color GetRelativeValue(object key, Color value)
        {
            throw new NotImplementedException();
        }
    }
}