using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _duration;

    [SerializeField] private int _value = 1000;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            _text.text = _value.ToString();
        }
    }

    private int _targetValue;
    private IEnumerator _co;

    private void Awake()
    {
        Value = _value;
    }

    public void SetValue(int value, bool force = false)
    {
        _targetValue = value;

        if (force)
        {
            Value = _targetValue;
            if (_co != null)
            {
                StopCoroutine(_co);
            }
            return;
        }

        if (_co == null)
        {
            StartCoroutine(_co = CoUpdate());
        }
    }

    private IEnumerator CoUpdate()
    {
        while (true)
        {
            int dir = _targetValue - Value;

            if (dir == 0)
            {
                break;
            }
            
            if (dir > 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            
            Value += dir;

            yield return new WaitForSeconds(_duration);
        }

        _co = null;
    }
}