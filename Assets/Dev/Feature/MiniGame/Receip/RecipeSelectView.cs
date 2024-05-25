using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSelectView : MonoBehaviour
{
    [SerializeField] private Button _testButton;
    [SerializeField] private RecipeData _data;
    [SerializeField] private float _openDuration;
    [SerializeField] private float _closeDuration;

    [SerializeField] public event Action<RecipeData> OnSelected;

    [field: SerializeField, HideInInspector]
    private Vector3 _openPivot;

    [field: SerializeField, HideInInspector]
    private Vector3 _closePivot;

    private bool _isClosed;


    [ButtonMethod]
    private void SetOpenPosition()
    {
        _openPivot = transform.position;
    }

    [ButtonMethod]
    private void SetClosePosition()
    {
        _closePivot = transform.position;
    }

    public async UniTask Open()
    {
        transform.position = _closePivot;

        DOTween.Kill(this);

        try
        {
            await transform
                .DOMove(_openPivot, _openDuration)
                .SetEase(Ease.Linear)
                .SetId(this)
                .AsyncWaitForCompletion()
                .AsUniTask()
                .WithCancellation(GlobalCancelation.PlayMode);

            await _testButton.OnClickAsync(GlobalCancelation.PlayMode);

            await Close();
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async UniTask Close()
    {
        transform.position = _openPivot;

        DOTween.Kill(this);

        try
        {
            await transform.DOMove(_closePivot, _closeDuration)
                .SetId(this)
                .AsyncWaitForCompletion()
                .AsUniTask()
                .WithCancellation(GlobalCancelation.PlayMode);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void Awake()
    {
        transform.position = _closePivot;
        
        _testButton.onClick.AddListener(() =>
        {
            OnSelected?.Invoke(_data);
        });
    }
}