using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;

public class NpcControllerTest : MonoBehaviour
{
    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), MustBeAssigned] 
    private NpcController _controller;
    
    [field: SerializeField, DefinedValues(nameof(GetNpcKeyList))]
    private string _npcKey;

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

    [SerializeField] private List<string> _npcKeyList;

    [ButtonMethod]
    public void ShowNpcList()
    {
        if (_controller == false || Application.isPlaying == false) return;

        List<Npc> npcs = new List<Npc>();

        foreach (var key in _npcKeyList)
        {
            var npc = _controller.AddNpc(key);
            npcs.Add(npc);

            npc.AnimateFadein();
        }
        
        _controller.SetNpcPosition(npcs, false);
    }
}
