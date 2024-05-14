using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class DialogueContext
{
    private List<string> _texts;
    private float _duration;
    private TMP_Text _text;

    private int _index;

    private CancellationTokenSource _source;

    public bool CanNext { get; private set; }
    
    public string CurrentText { get; private set; }

    public UniTask Next()
    {
        if (CanNext == false) return UniTask.CompletedTask;

        if (_source == null)
        {
            _source = new CancellationTokenSource();
        }
        else
        {
            _source.Cancel();
            _source = new CancellationTokenSource();
        }

        CurrentText = _texts[_index++];
        var task = _text.DoTextUniTask(CurrentText, _duration, _source.Token);


        if (_index >= _texts.Count)
        {
            CanNext = false;
        }

        return task;
    }

    public void Cancel()
    {
        _source?.Cancel();
        _source = null;
    }

    public DialogueContext(List<string> texts, float duration, TMP_Text text)
    {
        _texts = texts;
        _duration = duration;
        _text = text;
        CanNext = texts.Count > 0;
    }
}

[UnitTitle("DialogueContext")]
[UnitCategory("Time")]
public class DialogueContextUnit : WaitUnit
{
    [DoNotSerialize] [PortLabelHidden] public ValueInput Context { get; private set; }

    protected override void Definition()
    {
        base.Definition();

        Context = ValueInput<DialogueContext>("context");
    }

    protected override IEnumerator Await(Flow flow)
    {
        var context = flow.GetValue<DialogueContext>(Context);

        while (true)
        {
            if (context.CanNext == false)
            {
                if (InputManager.Actions.DialogueSkip.triggered)
                {
                    yield return exit;
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
            }

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            var task = context.Next();

            if (task.Status == UniTaskStatus.Faulted)
            {
                Debug.LogException(task.AsTask().Exception);
            }

            yield return new WaitUntil(() =>
                InputManager.Actions.DialogueSkip.triggered || task.Status != UniTaskStatus.Pending);

            if (task.Status != UniTaskStatus.Pending)
            {
                yield return new WaitUntil(() => InputManager.Actions.DialogueSkip.triggered);
            }
        }
    }
}

public class DialogueController : MonoBehaviour
{
    [field: SerializeField, InitializationField, MustBeAssigned]
    private GameObject _content;

    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private TMP_Text _text;


    public void Show()
    {
        _content.SetActive(true);
        _text.text = "";
    }

    public void Hide()
    {
        _content.SetActive(false);
        _text.text = "";
    }

    public DialogueContext BeginText(List<string> textList, float duration) =>
        new DialogueContext(
            textList,
            duration,
            _text);
}


public static class TMPProDotween
{
    public static UniTask DoTextUniTask(this TMPro.TMP_Text text, string endValue, float duration,
        CancellationToken? token = null)
    {
        return _DoText(text, endValue, duration, token);
    }

    private static async UniTask _DoText(TMPro.TMP_Text text, string endValue, float duration,
        CancellationToken? token = null)
    {
        string tempString = "";
        CancellationToken t = token == null
                ? GlobalCancelation.PlayMode
                : CancellationTokenSource.CreateLinkedTokenSource(GlobalCancelation.PlayMode, token.Value).Token
            ;

        text.text = "";

        for (int i = 0; i < endValue.Length; i++)
        {
            tempString += endValue[i];
            text.text = tempString;

            await UniTask.Delay((int)(duration / endValue.Length * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update,
                t);
        }
    }
}