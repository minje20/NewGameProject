using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;

public class RecipeSummaryController : MonoBehaviour
{
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private RecipeSummaryView _view;

    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private TMP_Text _text;
    
    public string Text
    {
        get => _text.text;
        set => _text.text = value;
    }

    public void Open()
    {
        _view.Open();
    }
    public void Close()
    {
        _view.Close();
    }
}
