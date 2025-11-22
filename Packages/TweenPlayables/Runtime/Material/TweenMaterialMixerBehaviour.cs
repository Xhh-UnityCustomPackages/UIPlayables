using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace TweenPlayables
{
    public class TweenMaterialMixerBehaviour : PlayableBehaviour
    {
        Material boundMaterial; //Timeline所使用到的材质
        Material defaultMaterial; //原始绑定的材质 但是是新创建的

        bool firstFrameHappened;
        
        public Material material => boundMaterial;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            boundMaterial = playerData as Material;
            if (boundMaterial == null) return;

            int inputCount = playable.GetInputCount();
            if (inputCount == 0) return;

            if (TweenMaterialLayerMixer.frameClean)
            {
                TweenMaterialLayerMixer.frameClean = false;

                if (firstFrameHappened)
                {
                    boundMaterial.CopyPropertiesFromMaterial(defaultMaterial);
                }
                else
                {
#if UNITY_EDITOR
                    // Prevent Unity from saving the previewed version of
                    // the bound material. Couldn't make it work via
                    // TrackAsset.GatherProperties().
                    UnityEditor.EditorApplication.quitting += ResetMaterial;
#endif

                    // Save original value
                    defaultMaterial = new Material(boundMaterial);
                    firstFrameHappened = true;
                }
            }

            // Get clips contributing to the current frame (weight > 0)
            List<int> activeClips = Enumerable
                    .Range(0, inputCount)
                    .Where(i => playable.GetInputWeight(i) > 0)
                    .ToList();

            if (activeClips.Count == 0) return;

            int clipIndex = activeClips[0];
            var clipData = GetBehaviour(playable, clipIndex);
            float clipWeight = playable.GetInputWeight(clipIndex);

            var mix = new TweenMaterialBehaviour(clipData);
            if (activeClips.Count > 1)
            {
                var next = GetBehaviour(playable, activeClips[1]);
                if (clipData.IsBlendableWith(next))
                {
                    mix.Lerp(next, clipData, clipWeight);
                }
                else
                {
                    var mix2 = new TweenMaterialBehaviour(next);
                    mix2.ApplyFromMaterial(boundMaterial);
                    mix2.Lerp(next, mix2, clipWeight);
                    mix2.ApplyToMaterial(boundMaterial);

                    // Current clip
                    mix.ApplyFromMaterial(boundMaterial);
                    mix.Lerp(mix, clipData, clipWeight);
                }
            }
            else if (clipWeight < 1)
            {
                mix.ApplyFromMaterial(boundMaterial);
                mix.Lerp(mix, clipData, clipWeight);
            }
            
            mix.ApplyToMaterial(boundMaterial);
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            firstFrameHappened = false;
            ResetMaterial();
        }

        void ResetMaterial()
        {
            if (boundMaterial && defaultMaterial)
                boundMaterial.CopyPropertiesFromMaterial(defaultMaterial);
        }

        static TweenMaterialBehaviour GetBehaviour(Playable playable, int inputPort) =>
                ((ScriptPlayable<TweenMaterialBehaviour>)playable.GetInput(inputPort)).GetBehaviour();
    }
}