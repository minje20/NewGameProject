using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManager : MonoBehaviour
{

    [SerializeField] private List<string> _npcKeys;
    [SerializeField] private List<string> _enterScriptCodeList;
    [SerializeField] private List<string> _exitScriptCodeList;

    public int CurrentEnterScriptCodeIndex { get; private set; }
    public int CurrentExitScriptCodeIndex { get; private set; }

    public bool IsGotoContinueScript => CurrentExitScriptCodeIndex >= _exitScriptCodeList.Count;
    public bool IsGotoContinueNpc => _currentNpcKeyIndex >= _npcKeys.Count;

    private int _currentNpcKeyIndex;

    public string GetCurrentEnterScriptCode()
    {
        string key = _enterScriptCodeList[CurrentEnterScriptCodeIndex++];
        
        CurrentEnterScriptCodeIndex = Mathf.Clamp(CurrentEnterScriptCodeIndex, 0, _enterScriptCodeList.Count - 1);
        
        return key;
    }
    
    public string GetCurrentExitScriptCode()
    {
        string key = _exitScriptCodeList[CurrentExitScriptCodeIndex++];
        
        return key;
    }

    public string GetCurrentNpcKey()
    {
        return _npcKeys[_currentNpcKeyIndex++];
    }
}
