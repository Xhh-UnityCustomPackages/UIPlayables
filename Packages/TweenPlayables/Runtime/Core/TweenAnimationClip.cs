using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TweenPlayables
{
    [Serializable]
    public abstract class TweenAnimationClip<TAnimationBehaviour> : PlayableAsset, ITimelineClipAsset
        where TAnimationBehaviour : PlayableBehaviour, new()
    {
        public TAnimationBehaviour behaviour;
        public TAnimationBehaviour Behaviour => behaviour;
        
        public ClipCaps clipCaps => ClipCaps.Extrapolation | ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TAnimationBehaviour>.Create(graph, behaviour);
            return playable;
        }
    }
}