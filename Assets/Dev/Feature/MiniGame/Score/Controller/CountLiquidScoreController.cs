using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;

public class CountLiquidScoreController : ScoreController
{
    [SerializeField] private LiquidScoreDisplayer.Parameter _displayerParameter;
    [SerializeField] private CountScoreBehaviour.Parameter _behaviourParameter;
    
    private CountScoreBehaviour _behaviour;
    private LiquidScoreDisplayer _displayer;

    private void Awake()
    {
        Setup();
        Release();
    }

    public void Setup()
    {
        Release();
        
        _behaviour = new(_behaviourParameter);
        _displayer = new(_displayerParameter);
    }

    public void Release()
    {
        _behaviour?.Dispose();
        _displayer?.Dispose();
        
        _behaviour = null;
        _displayer = null;
    }

    public void AddCount(int value)
    {
        _behaviour.CurrentScore += value;
    }

    public void SetCount(int value)
    {
        _behaviour.CurrentScore = value;
    }

    public void ShowLine(float pos)
    {
        _displayer.LineEnabled = true;
        _displayer.SetLinePosition(pos);
    }

    public UniTask DisplayResult()
    {
        EMiniGameScore score = _behaviour.GetCalculatedScore();
        return _displayer.Display(score);
    }
    
}
