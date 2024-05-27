using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectorController : MonoBehaviour, ISelectorUIController
{
    public Transform ClosePivot;
    public Transform OpenPivot;
    public Transform Content;
    public Button SelectingButton;
    public Button SkipButton;
    
    private List<SelectorButton> _buttons;

    private AsyncReactiveProperty<SelectorButton> _selectedSprite = new(null);


    private CancellationTokenSource _selectCancelation;

    private bool _canNoSelect;

    private void Awake()
    {
        transform.position = ClosePivot.position;

        _buttons = Content.GetComponentsInChildren<SelectorButton>().ToList();

        foreach (var button in _buttons)
        {
            button.Button.onClick.AddListener(() =>
            {
                var sbt = button.GetComponent<SelectorButton>();
                _selectedSprite.Value = sbt;
            });
        }

        SelectingButton.onClick.AddListener(() =>
        {
            if (_selectedSprite.Value == null && _canNoSelect == false) return;
            if (_selectedSprite.Value.Data is INoSelectedData && _canNoSelect == false) return;
            Close();
        });
        
        SkipButton.onClick.AddListener(() =>
        {
            _selectedSprite.Value = null;
            Close();
        });
    }

    public void Open(bool canNoSelect)
    {
        if (_selectCancelation != null) return;
        _canNoSelect = canNoSelect;
        _selectCancelation = new();
        
        var tween = transform.DOMove(OpenPivot.position, 1f);
    }

    private void Close()
    {
        if (_selectCancelation == null) return;
        
        if (_selectCancelation.IsCancellationRequested == false)
        {
            _selectCancelation?.Cancel();
        }

        _selectCancelation = null;
        transform.DOMove(ClosePivot.position, 1f);
    }

    public async UniTask<SelectorButton> GetSelectedItemOnChanged()
    {
        SelectorButton btn = null;

        try
        {
            btn = await _selectedSprite.WaitAsync(CancellationTokenSource.CreateLinkedTokenSource(
                GlobalCancelation.PlayMode,
                _selectCancelation.Token
            ).Token);
        }
        catch (NullReferenceException e)
        {
            return null;
        }

        return btn;
    }

    [ButtonMethod]
    private void SetPositionToClosePivots()
    {
        if (ClosePivot == false) return;

        ClosePivot.position = transform.position;
    }

    [ButtonMethod]
    private void SetPositionToOpenPivots()
    {
        if (OpenPivot == false) return;

        OpenPivot.position = transform.position;
    }
}