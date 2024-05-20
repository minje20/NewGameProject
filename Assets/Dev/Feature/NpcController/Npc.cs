using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private NpcCreationParameter _creationParameter;
    public NpcData NpcData => _creationParameter.NpcData;
    public NpcAnimationData AniData => _creationParameter.AnimationData;

    public NpcSlot Slot => _creationParameter.Slot;

    private SkeletonAnimation _animation;
    private MeshRenderer _renderer;

    public void Init(NpcCreationParameter parameter)
    {
        if (NpcData)
        {
            Debug.LogWarning($"이미 존재하는 npc({NpcData.Key})를 초기화하였습니다.");
            return;
        }

        _creationParameter = parameter;

        _animation = GetComponent<SkeletonAnimation>();
        _renderer = GetComponent<MeshRenderer>();

        _animation.skeletonDataAsset = NpcData.Asset;
        
        Debug.Assert(_renderer);

        StartCoroutine(OnUpdate());
    }

    private IEnumerator OnUpdate()
    {
        while (true)
        {
            _animation.AnimationName = "";
            _animation.AnimationName = "smoothing";

            yield return new WaitForSeconds(5f);
        }
    }

    public UniTask AnimateFadein()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.material.color = AniData.FadeoutColor;
        return _renderer.material.DOColor(AniData.FadeinColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
    public UniTask AnimateFadeout()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.material.color = AniData.FadeinColor;
        return _renderer.material.DOColor(AniData.FadeoutColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
}
