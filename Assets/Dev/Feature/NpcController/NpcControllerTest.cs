using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcControllerTest : MonoBehaviour
{
    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), MustBeAssigned] 
    private NpcController _controller;
    
    [field: SerializeField, DefinedValues(nameof(GetNpcKeyList))]
    private string _npcKey;

    [SerializeField] private string _aniKey;

    private string[] GetNpcKeyList()
    {
        if (_controller == false || Application.isPlaying == false) return new []{ "ERROR"};

        var arr = _controller.Table.Keys.ToArray();
        _npcKey = arr.Length > 0 ? arr[0] : "Missing";
        return arr;
    }
    
    [ButtonMethod]
    public string ShowNpc()
    {
        if (_controller == false || Application.isPlaying == false) return _npcKey;

        var npc = _controller.AddNpc(_npcKey);
        npc.AnimateFadein();
        
        return _npcKey;
    }
    [ButtonMethod]
    public string HideNpc()
    {
        if (_controller == false || Application.isPlaying == false) return _npcKey;

        if (_controller.TryGetNpc(_npcKey, out var npc))
        {
            npc.AnimateFadeout().ContinueWith(() =>
            {
                _controller.RemoveNpc(_npcKey);
            });
        }
        return _npcKey;
    }
    
    [ButtonMethod]
    public string Play()
    {
        if (_controller == false || Application.isPlaying == false) return _npcKey;

        if (_controller.TryGetNpc(_npcKey, out var npc))
        {
            npc.PlayAnimation(_aniKey);
        }
        return _npcKey;
    }
}
