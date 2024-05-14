using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcSlot
{
    public Transform Transform { get; private set; }
    public int Index { get; private set; }

    public NpcSlot(Transform transform, int index)
    {
        Transform = transform;
        Index = index;
    }
}

public struct NpcCreationParameter
{
    public NpcData NpcData;
    public NpcAnimationData AnimationData;
    public NpcSlot Slot;
}

public class NpcController : MonoBehaviour
{
    [FormerlySerializedAs("_table")] [field: SerializeField, Header("Npc 테이블"), MustBeAssigned]
    private NpcTable _npcTable;

    [field: SerializeField, OverrideLabel("Npc 애니메이팅 데이터"), MustBeAssigned]
    private NpcAnimationData _animationData;
    
    [field: SerializeField, Header("Npc 배치 슬롯"), MustBeAssigned]
    private List<Transform> _slots;

    private IReadOnlyDictionary<string, NpcData> _table;

    public IReadOnlyDictionary<string, NpcData> Table
    {
        get
        {
            if (_table == null)
            {
                _table = _npcTable.CreateTable();
            }

            return _table;
        }
    }

    private Dictionary<string, Npc> _createdNpcDict = new ();
    public Npc AddNpc(string npcKey)
    {
        if (Table.TryGetValue(npcKey, out var npcData) == false)
        {
            Debug.LogError($"npc key({npcKey}) missing");
            var errorNpc = NpcFactory.CreateErrorNpcObject(new NpcSlot(_slots[2], 2), npcKey);
            _createdNpcDict.Add(errorNpc.NpcData.Key, errorNpc);
            return errorNpc;
        }

        var npc = NpcFactory.CreateNpcObject(new NpcCreationParameter()
        {
            NpcData = npcData,
            AnimationData = _animationData,
            Slot = new(_slots[2], 2)
        });
        
        _createdNpcDict.Add(npc.NpcData.Key, npc);

        return npc;
    }

    public void RemoveNpc(string npcKey)
    {
        if (_createdNpcDict.TryGetValue(npcKey, out var npc) == false) 
        {
            Debug.LogError($"npc key({npcKey}) missing or not exist");
        }
        
        NpcFactory.DestroyNpc(npc);
        _createdNpcDict.Remove(npcKey);
    }

    public bool Contains(string npcKey)
        => _createdNpcDict.ContainsKey(npcKey);

    [CanBeNull]
    public Npc GetNpc(string npcKey)
        => _createdNpcDict.GetValueOrDefault(npcKey);

    public bool TryGetNpc(string npcKey, out Npc npc)
    {
        npc = _createdNpcDict.GetValueOrDefault(npcKey);

        return npc;
    }

    public List<Npc> GetNpcAll()
        => _createdNpcDict.Values.ToList();
}
