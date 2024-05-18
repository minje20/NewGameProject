using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class CountScoreBehaviour : IScoreBehaviour
{
    [Serializable]
    public struct Parameter
    {
        [Serializable]
        public struct T
        {
            public EMiniGameScore Score;
            public Vector2Int Range;
        }
        
        //TODO: 더미 구현. 향후 scriptableObject로 변경
        public int TargetCount;
        public List<T> Range;
    }
    
    private const int STANDARD_SCORE = 100;

    private readonly int _targetCount;
    private readonly List<(EMiniGameScore, Vector2Int)> _range;
    
    
    private int _currentScore;

    public int CurrentScore
    {
        get => _currentScore;
        set => _currentScore = value;
    }
    
    public CountScoreBehaviour(Parameter parameter)
    {
        _targetCount = parameter.TargetCount;
        _range = parameter.Range.Select(x=>(x.Score, x.Range)).ToList();

        ValidateRange();

        _currentScore = 0;
    }
    public EMiniGameScore GetCalculatedScore()
    {
        Debug.Assert(_targetCount > 0);
        
        float factor = (float)_currentScore / (float)_targetCount;

        int score = Mathf.CeilToInt(STANDARD_SCORE * factor);

        return GetIntToEnumScore(score);
    }

    private bool IsIncludeInRange(int value, Vector2Int range)
    {
        return value >= range.x && value <= range.y;
    }
    private void ValidateRange()
    {
        
        for (int i = 0; i < _range.Count; i++)
        {
            Vector2Int firstRange = _range[i].Item2;
            
            
            for (int j = 0; j < _range.Count; j++)
            {
                if (i == j) continue;
                Vector2Int secondRange = _range[j].Item2;

                if (IsIncludeInRange(firstRange.x, secondRange) || IsIncludeInRange(firstRange.y, secondRange))
                {
                    Debug.LogError($"invalid range: index:{i}, {j}, value: {firstRange}, {secondRange}");
                }
            }
        }
    }
    
    private EMiniGameScore GetIntToEnumScore(int value)
    {
        foreach (var range in _range)
        {
            if (IsIncludeInRange(value, range.Item2))
            {
                return range.Item1;
            }
        }

        return EMiniGameScore.Bad;
    }

    public void Dispose()
    {
        
    }
}
