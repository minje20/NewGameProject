using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CountTextScoreController : MonoBehaviour
{
    [SerializeField] TextScoreDisplayer.Parameter _parameter;
    
    private CountScoreBehaviour _behaviour;
    private TextScoreDisplayer _displayer;

    public EMiniGameScore CurrentScore => _behaviour.GetCalculatedScore();

    private void Awake()
    {
        Setup(null, new TextScoreDisplayer.ValueParameter());
        
        Release();
    }

    public void Setup(CountScoreBehaviour.Parameter? behaviourParameter, TextScoreDisplayer.ValueParameter displayerParameter)
    {
        Release();

        if (behaviourParameter == null)
        {
            behaviourParameter = new CountScoreBehaviour.Parameter()
            {
                Range = new(),
                TargetCount = int.MaxValue
            };
        }
        
        
        _behaviour = new(behaviourParameter.Value);
        _displayer = new(_parameter, displayerParameter);
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
        _displayer.SetCount(_behaviour.CurrentScore);
    }

    public void SetCount(int value)
    {
        _behaviour.CurrentScore = value;
        _displayer.SetCount(_behaviour.CurrentScore);
    }

    public void SetEnableText(bool value)
    {
        _displayer.CountingTextEnabled = value;
    }

    public UniTask DisplayResult()
    {
        EMiniGameScore score = _behaviour.GetCalculatedScore();
        
        return _displayer.Display(score);
    }

    private void __MiniGame_Reset__()
    {
        Release();
    }

}
