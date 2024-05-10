using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public NpcData NpcData { get; private set; }
    public NpcAnimationData AniData { get; private set; }

    private SpriteRenderer _renderer;

    public void Init(NpcCreationParameter parameter)
    {
        if (NpcData)
        {
            Debug.LogWarning($"이미 존재하는 npc({NpcData.Key})를 초기화하였습니다.");
            return;
        }
        
        NpcData = parameter.NpcData;
        AniData = parameter.AnimationData;

        _renderer = GetComponent<SpriteRenderer>();
        
        Debug.Assert(_renderer);
    }

    public UniTask AnimateFadein()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.color = AniData.FadeoutColor;
        return _renderer.DOColor(AniData.FadeinColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask();
    }
    public UniTask AnimateFadeout()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.color = AniData.FadeinColor;
        return _renderer.DOColor(AniData.FadeoutColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask();
    }
}
