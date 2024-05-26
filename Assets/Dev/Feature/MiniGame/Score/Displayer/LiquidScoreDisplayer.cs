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
        public LineRenderer Line;
        public Transform DisplayPivot;

        public Transform AUpPivot;
        public Transform BUpPivot;
        
        public Transform ADownPivot;
        public Transform BDownPivot;
    }

    [Serializable]
    public struct ValueParameter
    {
        [Range(0f, 1f)]
        public float LinePosition;
    }

    private TMP_Text _enumScoreText;
    private LineRenderer _line;
    private Transform _displayPivot;
    
    private Vector3 _aUpPivot;
    private Vector3 _bUpPivot;
    private Vector3 _aDownPivot;
    private Vector3 _bDownPivot;

    private (Vector3, Vector3) GetEvaluatePoints(float t)
    {
        t = 1f - t;
        Vector3 p1 = Vector3.Lerp(_aUpPivot, _aDownPivot, t);
        Vector3 p2 = Vector3.Lerp(_bUpPivot, _bDownPivot, t);

        return (p1, p2);
    }

    public LiquidScoreDisplayer(Parameter parameter)
    {
        _enumScoreText = parameter.EnumScoreText;
        _line = parameter.Line;
        _displayPivot = parameter.DisplayPivot;

        _aUpPivot = parameter.AUpPivot.position;
        _bUpPivot = parameter.BUpPivot.position;
        _aDownPivot = parameter.ADownPivot.position;
        _bDownPivot = parameter.BDownPivot.position;
    }

    public void SetLinePosition(float t)
    {
        var tuple = GetEvaluatePoints(t);
        Vector3 p1 = tuple.Item1;
        Vector3 p2 = tuple.Item2;

        _line.positionCount = 2;
        _line.SetPositions(new []{p1, p2});
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