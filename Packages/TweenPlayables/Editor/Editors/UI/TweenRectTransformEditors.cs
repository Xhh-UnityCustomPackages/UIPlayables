using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Playables;

namespace TweenPlayables.Editor
{
    [CustomTimelineEditor(typeof(TweenRectTransformTrack))]
    public sealed class TweenRectTransformTrackEditor : TweenAnimationTrackEditor
    {
        public override Color TrackColor => Styles.UGUIColor;
        public override Texture2D TrackIcon => Styles.RectTransformIcon;
        public override string DefaultTrackName => "Tween RectTransform Track";
    }

    [CustomTimelineEditor(typeof(TweenRectTransformClip))]
    public sealed class TweenRectTransformClipEditor : TweenAnimationClipEditor
    {
        public override string DefaultClipName => "Tween RectTransform";
        public override Color ClipColor => Styles.UGUIColor;
        public override Texture2D ClipIcon => Styles.RectTransformIcon;
    }

    [CustomPropertyDrawer(typeof(TweenRectTransformBehaviour))]
    public sealed class TweenRectTransformBehaviourDrawer : TweenAnimationBehaviourDrawer
    {
        static readonly string[] parameters = new string[]
        {
            "anchoredPosition", "sizeDelta", "rotation", "scale", "anchor", "pivot"
        };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = 9f;
            foreach (var propertyName in GetPropertyNames())
            {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(propertyName));
                height += 2f;
            }

            height += 20f;

            return height;
        }

        protected override IEnumerable<string> GetPropertyNames() => parameters;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            GUILayout.BeginHorizontal();
            //添加按钮读取位置信息
            if (GUILayout.Button("重置AnchorPosition"))
            {
                var anchoredPosition = property.FindPropertyRelative("anchoredPosition");
                var track = TimelineEditor.selectedClip.GetParentTrack() as TweenRectTransformTrack;

                PlayableDirector targetDirector = this.FindTargetDirector();

                if (targetDirector != null)
                {
                    var target = targetDirector.GetGenericBinding(track) as RectTransform;
                    anchoredPosition.FindPropertyRelative("startValue").vector3Value = target.anchoredPosition3D;
                    anchoredPosition.FindPropertyRelative("endValue").vector3Value = target.anchoredPosition3D;
                }
            }
            
            if (GUILayout.Button("重置SizeDelta"))
            {
                var anchoredPosition = property.FindPropertyRelative("sizeDelta");
                var track = TimelineEditor.selectedClip.GetParentTrack() as TweenRectTransformTrack;

                PlayableDirector targetDirector = this.FindTargetDirector();

                if (targetDirector != null)
                {
                    var target = targetDirector.GetGenericBinding(track) as RectTransform;

                    anchoredPosition.FindPropertyRelative("startValue").vector2Value = target.sizeDelta;
                    anchoredPosition.FindPropertyRelative("endValue").vector2Value = target.sizeDelta;
                }
            }

            if (GUILayout.Button("重置Scale"))
            {
                var scale = property.FindPropertyRelative("scale");
                var track = TimelineEditor.selectedClip.GetParentTrack() as TweenRectTransformTrack;

                PlayableDirector targetDirector = this.FindTargetDirector();

                if (targetDirector != null)
                {
                    var target = targetDirector.GetGenericBinding(track) as RectTransform;

                    scale.FindPropertyRelative("startValue").vector3Value = target.localScale;
                    scale.FindPropertyRelative("endValue").vector3Value = target.localScale;
                }
            }

            // if (GUILayout.Button("重置Anchor"))
            // {
            //     var anchoredPosition = property.FindPropertyRelative("anchor");
            //     var track = TimelineEditor.selectedClip.GetParentTrack() as TweenRectTransformTrack;
            //
            //     PlayableDirector targetDirector = this.FindTargetDirector();
            //
            //     if (targetDirector != null)
            //     {
            //         var target = targetDirector.GetGenericBinding(track) as RectTransform;
            //
            //         var targetValue = new Vector4(target.anchorMin.x, target.anchorMin.y, target.anchorMax.x, target.anchorMax.y);
            //         anchoredPosition.FindPropertyRelative("startValue").vector4Value = targetValue;
            //         anchoredPosition.FindPropertyRelative("endValue").vector4Value = targetValue;
            //     }
            // }
            
            if (GUILayout.Button("重置Pivot"))
            {
                var pivot = property.FindPropertyRelative("pivot");
                var track = TimelineEditor.selectedClip.GetParentTrack() as TweenRectTransformTrack;

                PlayableDirector targetDirector = this.FindTargetDirector();

                if (targetDirector != null)
                {
                    var target = targetDirector.GetGenericBinding(track) as RectTransform;

                    pivot.FindPropertyRelative("startValue").vector2Value = target.pivot;
                    pivot.FindPropertyRelative("endValue").vector2Value = target.pivot;
                }
            }

            GUILayout.EndHorizontal();
        }

        PlayableDirector FindTargetDirector()
        {
            var track = TimelineEditor.selectedClip.GetParentTrack() as TweenRectTransformTrack;
            var timelineAsset = track.timelineAsset;

            var directors = GameObject.FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None);
            PlayableDirector targetDirector = null;
            foreach (var director in directors)
            {
                if (director.playableAsset == timelineAsset)
                {
                    return director;
                }
            }

            return null;
        }
    }
}