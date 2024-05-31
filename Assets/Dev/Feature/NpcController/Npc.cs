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

    private List<BaseNpcBehaviour> _behaviours = new();
    private NpcSpriteBehaviour _spriteBehaviour;


    public void Init(NpcCreationParameter parameter)
    {
        if (NpcData)
        {
            Debug.LogWarning($"이미 존재하는 npc({NpcData.Key})를 초기화하였습니다.");
            return;
        }
        
        _creationParameter = parameter;

        _behaviours.Add(_spriteBehaviour = BaseNpcBehaviour.Create<NpcSpriteBehaviour>(gameObject, NpcData));
        _behaviours.Add(BaseNpcBehaviour.Create<NpcSpineBehaviour>(gameObject, NpcData));
        _behaviours.Add(BaseNpcBehaviour.Create<NpcSpriteAnimationBehaviour>(gameObject, NpcData));
    }
    
    public void PlayAnimation(string key)
    {
        if (_behaviours == null) return;

        if (key == "")
        {
            return;
        }

        foreach (BaseNpcBehaviour behaviour in _behaviours)
        {
            behaviour.Stop();
        }
        foreach (BaseNpcBehaviour behaviour in _behaviours)
        {
            if (behaviour.PlayAnimation(key)) return;
        }
    }

    public void SetDefault()
    {
        foreach (BaseNpcBehaviour behaviour in _behaviours)
        {
            behaviour.Stop();
        }
        _spriteBehaviour.SetDefault();
    }

    public UniTask AnimateFadein()
    {
        SetDefault();
        
        _spriteBehaviour.Renderer.material.color = AniData.FadeoutColor;
        return _spriteBehaviour.Renderer.material.DOColor(AniData.FadeinColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
    public UniTask AnimateFadeout()
    {
        SetDefault();

        _spriteBehaviour.Renderer.material.color = AniData.FadeinColor;
        return _spriteBehaviour.Renderer.material.DOColor(AniData.FadeoutColor, AniData.FadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
}
