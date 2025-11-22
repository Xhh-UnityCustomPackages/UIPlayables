#if UNITY_EDITOR
using System.ComponentModel;
#endif
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace TweenPlayables
{
    [TrackBindingType(typeof(RectMask2D))]
    [TrackClipType(typeof(TweenRectMask2DClip))]
#if UNITY_EDITOR
    [DisplayName("Tween Playables/UI/Rect Mask 2D")]
#endif
    public class TweenRectMask2DTrack : TweenAnimationTrack<RectMask2D, TweenRectMask2DMixerBehaviour, TweenRectMask2DBehaviour>
    {
    }
}