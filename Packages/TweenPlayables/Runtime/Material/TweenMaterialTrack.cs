using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
#if UNITY_EDITOR
using System.ComponentModel;
#endif

namespace TweenPlayables
{
    [TrackBindingType(typeof(Material))]
    [TrackClipType(typeof(TweenMaterialClip))]
#if UNITY_EDITOR
    [DisplayName("Tween Playables/Material")]
#endif
    public class TweenMaterialTrack : TrackAsset, ILayerable
    {
        const string EMPTY_SLOT_NAME = "Empty";

        public Playable CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TweenMaterialLayerMixer>.Create(graph, inputCount);
        }

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<TweenMaterialMixerBehaviour>.Create(graph, inputCount);
            TweenMaterialMixerBehaviour behaviour = mixer.GetBehaviour();

            foreach (TimelineClip clip in GetClips())
            {
                TweenMaterialBehaviour data = ((TweenMaterialClip)clip.asset).Behaviour;
                clip.displayName = BuildClipName(data);

                data.mixer = behaviour;
            }

            return mixer;
        }

        private static string BuildClipName(TweenMaterialBehaviour data)
        {
            if (string.IsNullOrWhiteSpace(data.propertyName))
            {
                return EMPTY_SLOT_NAME;
            }

            return $"{data.propertyName} [{data.propertyType}]";
        }
    }
}