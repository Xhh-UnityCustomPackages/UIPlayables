using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

[MenuEntry("Editing/Align End (对齐End)", MenuPriority.ClipActionSection.findSourceAsset + 1), UsedImplicitly]
class AlignEnd : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        int count = clips.Count();
        return count > 1 ? ActionValidity.Valid : ActionValidity.Invalid;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        var finalEnd = double.MinValue;
        clips.ForEach(x => { finalEnd = Math.Max(finalEnd, x.end); });

        clips.ForEach(x => x.duration = finalEnd - x.start);
        return true;
    }
}

[MenuEntry("Editing/Align With Previous Clip(吸附前一个End)", MenuPriority.ClipActionSection.findSourceAsset + 2), UsedImplicitly]
class AlignStrat : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        int count = clips.Count();
        return count > 0 ? ActionValidity.Valid : ActionValidity.Invalid;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        clips.ForEach(x =>
        {
            var track = x.GetParentTrack();
            var clips = track.GetClips();
            TimelineClip previousClip = null;
        
            // 查找当前Clip的前一个Clip
            foreach (var clip in clips)
            {
                if (clip == x)
                    break;
                previousClip = clip;
            }

            if (previousClip != null)
            {
                var end = x.end;
                if (Math.Abs(x.end - previousClip.end) > 0.001f)
                {
                    x.start = previousClip.end;
                    x.duration = end - x.start;
                }
            }

        });
        return true;
    }
}


