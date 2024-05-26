using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



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

    private UniTask _prevTask = UniTask.CompletedTask;

    public UniTask Next()
    {
        if (CanNext == false) return UniTask.CompletedTask;

        if (_source == null)
        {
            _source = new CancellationTokenSource();
        }
        else
        {
            string str = string.Empty;
            if (_prevTask.Status == UniTaskStatus.Pending)
            {
                str = CurrentText;
            }
            
            _source.Cancel();
            _source = new CancellationTokenSource();

            if (string.IsNullOrEmpty(str) == false)
            {
                _text.text = str;
                return UniTask.CompletedTask;
            }
        }

        var item = _texts[_index++];
        CurrentText = item.Text;

        _upDir.gameObject.SetActive(!item.IsMaster);
        _downDir.gameObject.SetActive(item.IsMaster);
        
        var task = _text.DoTextUniTask(CurrentText, _duration, _source.Token);
        _prevTask = task;

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