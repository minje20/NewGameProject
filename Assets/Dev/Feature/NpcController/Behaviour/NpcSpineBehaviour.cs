using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;

public class NpcSpineBehaviour : BaseNpcBehaviour
{
    private SkeletonAnimation _animation;
    protected override void Init()
    {
        gameObject.AddComponent<MeshFilter>();
        var renderer = gameObject.AddComponent<MeshRenderer>();
        _animation = gameObject.AddComponent<SkeletonAnimation>();
        renderer.sortingLayerName = "Npc";
    }
    public override bool PlayAnimation(string key)
    {
        if (_animation == false) return false;

        NpcData.SpineKeyPair.KeyPair keyPair = null;
        var pair = Data.SpineAsset.Find(x => x.ContainAnimationKeys.Exists(y =>
        {
            if (y.Key == key)
            {
                keyPair = y;
                return true;
            }

            return false;
        }));
        
        if (pair == null) return false;
        
        gameObject.SetActive(true);
        _animation.skeletonDataAsset = pair.Asset;
        _animation.AnimationName = "";
        _animation.AnimationName = keyPair.SpineKey;
        transform.localPosition = pair.Offset;
        
        return true;
    }

    public override void Stop()
    {
        gameObject.SetActive(false);
    }
}
