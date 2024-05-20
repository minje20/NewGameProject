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
using UnityEngine.UI;

[Serializable]
public class DialogueItem
{
    public string Text { get; private set; }
    public bool IsMaster { get; private set; }

    public DialogueItem(string text, bool isMaster)
    {
        Text = text;
        IsMaster = isMaster;
    }
}

[Serializable]
public class DialogueContext
{
    private List<DialogueItem> _texts;
    private float _duration;
    private TMP_Text _text;
    private Image _upDir;
    private Image _downDir;

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

        var item = _texts[_index++];
        CurrentText = item.Text;

        _upDir.gameObject.SetActive(!item.IsMaster);
        _downDir.gameObject.SetActive(item.IsMaster);
        
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

    public DialogueContext(List<DialogueItem> texts, float duration, TMP_Text text, Image upDir, Image downDir)
    {
        _texts = texts;
        _duration = duration;
        _text = text;
        CanNext = texts.Count > 0;
        _upDir = upDir;
        _downDir = downDir;
    }
}

public class DialogueItemMaker : Unit
{
    [DoNotSerialize] [PortLabelHidden] public ControlInput _cInput { get; private set; }
    [DoNotSerialize] [PortLabelHidden] public ControlOutput _cOutput { get; private set; }
    
    [DoNotSerialize] [PortLabelHidden] public ValueInput TextList { get; private set; }
    [DoNotSerialize] [PortLabelHidden] public ValueInput MasterList { get; private set; }
    [DoNotSerialize] [PortLabelHidden] public ValueOutput Result { get; private set; }

    private List<DialogueItem> _result;
    
    protected override void Definition()
    {
        _cInput = ControlInput("", Make);
        _cOutput = ControlOutput("");

        TextList = ValueInput<List<string>>("text");
        MasterList = ValueInput<List<bool>>("is master");
        Result = ValueOutput<List<DialogueItem>>("items", (x)=>_result);
    }

    private ControlOutput Make(Flow flow)
    {
        var texts = flow.GetValue<List<string>>(TextList);
        var masters = flow.GetValue<List<bool>>(MasterList);

        var result = new List<DialogueItem>(texts.Count);

        for (int i = 0; i < Mathf.Min(texts.Count, masters.Count); i++)
        {
            result.Add(new DialogueItem(texts[i], masters[i]));
        }

        _result = result;

        return _cOutput;
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


    [field: SerializeField, InitializationField, MustBeAssigned]
    private Image _upDirection;

    [field: SerializeField, InitializationField, MustBeAssigned]
    private Image _downDirection;
    

    [field: SerializeField, InitializationField, MustBeAssigned]
    private Image _skipTip;

    [SerializeField] private float _skipTwinkleDuration = 0.5f;

    private void Awake()
    {
        StartCoroutine(CoUpdate());
    }

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

    public DialogueContext BeginText(List<DialogueItem> textList, float duration) =>
        new DialogueContext(
            textList,
            duration,
            _text,
            _upDirection,
            _downDirection
        );

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            _skipTip.gameObject.SetActive(true);
            yield return new WaitForSeconds(_skipTwinkleDuration);
            _skipTip.gameObject.SetActive(false);
            yield return new WaitForSeconds(_skipTwinkleDuration);
        }
    } 
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