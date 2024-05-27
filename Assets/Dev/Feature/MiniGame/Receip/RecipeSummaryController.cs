using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using TMPro;
using UnityEngine;

public class RecipeSummaryController : MonoBehaviour
{
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private RecipeSummaryView _view;
    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), InitializationField, MustBeAssigned]
    private BarController _barController;

    [field: SerializeField, Multiline] private string _iceTextTemplate;
    [field: SerializeField, Multiline] private string _measurementTextTemplate;
    [field: SerializeField, Multiline] private string _shakingTextTemplate;
    
    public string Text
    {
        get => _view.Text.text;
        set => _view.Text.text = value;
    }

    public void Open()
    {
        _view.Open();
    }
    public void Close()
    {
        _view.Close();
    }
    
    private void __MiniGame_Reset__()
    {
        Text = "";
    }
    
    private void Update()
    {
        if (_view.IsClosed) return;

        var context = _barController.Context;

        var list = context
            .MeasuredDrinkTable
            .Select(x => (x.Key, x.Value))
            .ToList();

        string str = "";
        
        str += _iceTextTemplate.FormatWithPlaceholder(
            ("ice_max_count", context.IceMaxCount),
            ("ice_current_count", context.CurrentIceCount),
            ("ice_score", context.IsIceEnd ? context.IceScore : "")
        ) + "\n";

        for (int i = 0; i < list.Count; i++)
        {
            str += _measurementTextTemplate.FormatWithPlaceholder(
                ("measurement_name", list[i].Item1.Name),
                ("measurement_score", list[i].Item2.IsEnd ? list[i].Item2.Score : "")
            )+ "\n";
        }

        str += _shakingTextTemplate.FormatWithPlaceholder(
            ("shaking_score", context.IsShakeEnd ? context.ShakeScore : "")
        ) + "\n";

        Text = str;

    }
}
