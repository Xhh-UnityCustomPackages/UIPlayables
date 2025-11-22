
using UnityEngine.Playables;

namespace TweenPlayables
{
    public class TweenMaterialLayerMixer : PlayableBehaviour
    {
        public static bool frameClean = true;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            // The layer mixer is executed after all track layer mixers.
            // All that is left to do is to tell the first mixer of the next
            // frame that it's the first one
            frameClean = true;
        }
    }
}