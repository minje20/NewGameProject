using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IndieLINY.MessagePipe;
using MyBox;
using UnityEngine;


public class CountScorePipeEvent : IMessagePipeEvent
{
    public CountScoreBehaviour.Parameter Parameter;
}

public class CountScoreChannel : PubSubMessageChannel<CountScorePipeEvent>
{
}

public class RecipeSelectController : MonoBehaviour, IMessagePipePublisher<CountScorePipeEvent>
{
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private RecipeSelectView _view;

    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), InitializationField, MustBeAssigned]
    private BarController _barController;

    private void OnEnable()
    {
        _view.OnSelected += OnSelected;
    }

    private void OnDisable()
    {
        _view.OnSelected -= OnSelected;
    }

    public async UniTask<RecipeData> Open()
    {
        await _view.Open();
        return _barController.CurrentRecipeData;
    }

    private void OnSelected(RecipeData data)
    {
        _barController.CurrentRecipeData = data;
        
    }
}
