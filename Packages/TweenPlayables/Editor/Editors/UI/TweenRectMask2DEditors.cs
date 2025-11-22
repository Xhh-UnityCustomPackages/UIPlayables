using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace TweenPlayables.Editor
{

    [CustomTimelineEditor(typeof(TweenRectMask2DTrack))]
    public sealed class TweenRectMask2DTrackEditor : TweenAnimationTrackEditor
    {
        public override Color TrackColor => Styles.UGUIColor;
        public override Texture2D TrackIcon => Styles.OutlineIcon;
        public override string DefaultTrackName => "Tween RectMask2D Track";
    }
    
    [CustomTimelineEditor(typeof(TweenRectMask2DClip))]
    public sealed class TweenRectMask2DClipEditor : TweenAnimationClipEditor
    {
        public override Color ClipColor => Styles.UGUIColor;
        public override Texture2D ClipIcon => Styles.OutlineIcon;
        public override string DefaultClipName => "Tween RectMask2D";
    }

    [CustomPropertyDrawer(typeof(TweenRectMask2DBehaviour))]
    public sealed class TweenRectMask2DBehaviourDrawer : TweenAnimationBehaviourDrawer
    {
        static readonly string[] parameters = new string[]
        {
            "padding"
        };

        protected override IEnumerable<string> GetPropertyNames() => parameters;
    }
}
