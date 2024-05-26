using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using TMPro;
using UnityEngine;

public class TextScoreDisplayer : IScoreDisplayer
{
    [Serializable]
    public struct Parameter
    {
        public TMP_Text CountingText;
        public TMP_Text ResultText;
    }
    [Serializable]
    public struct ValueParameter
    {
        public int MaxCount;
    }

    private Parameter _parameter;
    private ValueParameter _valueParameter;

    public TextScoreDisplayer(Parameter parameter, ValueParameter valueParameter)
    {
        _parameter = parameter;
        _valueParameter = valueParameter;

        SetCount(0);
    }

    public bool CountingTextEnabled
    {
        get => _parameter.CountingText.gameObject.activeSelf;
        set => _parameter.CountingText.gameObject.SetActive(value);
    }
    public bool ResultTextEnabled
    {
        get => _parameter.ResultText.gameObject.activeSelf;
        set => _parameter.ResultText.gameObject.SetActive(value);
    }

    public void SetCount(int count)
    {
        _parameter.CountingText.text = $"{count}/{_valueParameter.MaxCount}";
    }

    public async UniTask Display(EMiniGameScore score)
    {
        _parameter.ResultText.text = score.ToString();

        ResultTextEnabled = true;
        CountingTextEnabled = false;
        await UniTask.Delay(1000, DelayType.DeltaTime, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
        ResultTextEnabled = false;
    }


    public void Dispose()
    {
        ResultTextEnabled = false;
        CountingTextEnabled = false;
    }
}
