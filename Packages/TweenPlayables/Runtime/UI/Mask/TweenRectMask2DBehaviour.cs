using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TweenPlayables
{
    [Serializable]
    public class TweenRectMask2DBehaviour : TweenAnimationBehaviour<RectMask2D>
    {
        [SerializeField] Vector4TweenParameter padding;
        
        public ReadOnlyTweenParameter<Vector4> Padding => padding;

        public override void OnTweenInitialize(RectMask2D playerData)
        {
            padding.SetInitialValue(playerData, playerData.padding);
        }
    }
}
