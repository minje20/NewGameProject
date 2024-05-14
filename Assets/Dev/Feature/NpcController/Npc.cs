using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private NpcCreationParameter _creationParameter;
    public NpcData NpcData => _creationParameter.NpcData;
    public NpcAnimationData AniData => _creationParameter.AnimationData;

    public NpcSlot Slot => _creationParameter.Slot;

    private SpriteRenderer _renderer;

    public void Init(NpcCreationParameter parameter)
    {
        if (NpcData)
        {
            Debug.LogWarning($"이미 존재하는 npc({NpcData.Key})를 초기화하였습니다.");
            return;
        }

        _creationParameter = parameter;

        _renderer = GetComponent<SpriteRenderer>();
        
        Debug.Assert(_renderer);
    }

    public UniTask AnimateFadein()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.color = AniData.FadeoutColor;
        return _renderer.DOColor(AniData.FadeinColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
    public UniTask AnimateFadeout()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.color = AniData.FadeinColor;
        return _renderer.DOColor(AniData.FadeoutColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
}
