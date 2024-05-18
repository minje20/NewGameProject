using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LiquidScoreDisplayer : IScoreDisplayer
{
    [Serializable]
    public struct Parameter
    {
        public TMP_Text EnumScoreText;
        public SpriteRenderer Line;
        public Transform DisplayPivot;
    }

    private TMP_Text _enumScoreText;
    private SpriteRenderer _line;
    private Transform _displayPivot;

    public LiquidScoreDisplayer(Parameter parameter)
    {
        _enumScoreText = parameter.EnumScoreText;
        _line = parameter.Line;
        _displayPivot = parameter.DisplayPivot;
    }

    public void SetLinePosition(Vector3 position)
    {
        _line.transform.position = position;
    }

    public async UniTask Display(EMiniGameScore score)
    {
        _enumScoreText.text = score.ToString();

        ScoreTextEnabled = true;
        _enumScoreText.transform.position = _displayPivot.position;
        await UniTask.Delay(1000, DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
        ScoreTextEnabled = false;
    }

    private bool _lineEnabled;

    public bool LineEnabled
    {
        get => _lineEnabled;
        set
        {
            _lineEnabled = value;
            _line.gameObject.SetActive(value);
        }
    }

    private bool _scoreTextEnabled;

    public bool ScoreTextEnabled
    {
        get => _scoreTextEnabled;
        set
        {
            _lineEnabled = value;
            _enumScoreText.gameObject.SetActive(value);
        }
    }

    public void Dispose()
    {
        LineEnabled = ScoreTextEnabled = false;
    }
}