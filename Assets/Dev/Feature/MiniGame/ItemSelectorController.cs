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
    public DrinkPosition DrinkPosition;
    public IngredientSelectionUI View;
    
    private AsyncReactiveProperty<DrinkData> _selectedSprite = new(null);

    private CancellationTokenSource _selectCancelation;

    private bool _canNoSelect;

    private void Awake()
    {
        transform.position = ClosePivot.position;

        SelectingButton.onClick.AddListener(() =>
        {
            _selectedSprite.Value = View.CurrentData;
            DrinkPosition.Data = View.CurrentData;
            
            Close();
        });
        
        SkipButton.onClick.AddListener(() =>
        {
            _selectedSprite.Value = null;
            DrinkPosition.Data = null;
            Close();
        });
    }

    private void Update()
    {
        if (_selectCancelation != null)
        {
            if (InputManager.Actions.ShakingMiniGameInteraction.triggered)
            {
                _selectedSprite.Value = View.CurrentData;
                DrinkPosition.Data = View.CurrentData;
                Close();
            }
            else
            {
                _selectedSprite.Value = View.CurrentData;
                DrinkPosition.Data = View.CurrentData;
            }
        }
    }

    public void Open(bool canNoSelect)
    {
        if (_selectCancelation != null) return;
        _canNoSelect = canNoSelect;
        _selectCancelation = new();
        
        Content.gameObject.SetActive(true);
        
        var tween = transform.DOMove(OpenPivot.position, 1f).SetId(this);
    }

    private void Close()
    {
        if (_selectCancelation == null) return;
        
        if (_selectCancelation.IsCancellationRequested == false)
        {
            _selectCancelation?.Cancel();
        }

        _selectCancelation = null;
        View.MoveStop();
        transform.DOMove(ClosePivot.position, 1f).OnComplete(()=>
            Content.gameObject.SetActive(false)).SetId(this);
    }

    private void __MiniGame_Reset__()
    {
        Close();
        DOTween.Kill(this);
        transform.position = ClosePivot.position;
    }

    public async UniTask<DrinkData> GetSelectedItemOnChanged(CancellationToken token)
    {
        DrinkData btn = null;

        try
        {
            btn = await _selectedSprite.WaitAsync(CancellationTokenSource.CreateLinkedTokenSource(
                GlobalCancelation.PlayMode,
                _selectCancelation.Token,
                token
            ).Token);
        }
        catch (NullReferenceException)
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