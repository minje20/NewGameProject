using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;

public class RecipeSelectController : MonoBehaviour
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
