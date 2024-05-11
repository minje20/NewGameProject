using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;


public class NpcController : MonoBehaviour
{
    [FormerlySerializedAs("_table")] [field: SerializeField, Header("Npc 테이블"), MustBeAssigned]
    private NpcTable _npcTable;

    [field: SerializeField, OverrideLabel("Npc 애니메이팅 데이터"), MustBeAssigned]
    private NpcAnimationData _animationData;
    
    [field: SerializeField, Header("Npc 배치 슬롯"), MustBeAssigned]
    private List<Transform> _slots;

    public IReadOnlyDictionary<string, NpcData> Table { get; private set; }

    private Dictionary<string, Npc> _createdNpcDict = new ();

    private void Awake()
    {
        Table = _npcTable.CreateTable();
    }

    public Npc AddNpc(string npcKey)
    {
        if (Table.TryGetValue(npcKey, out var npcData) == false)
        {
            Debug.LogError($"npc key({npcKey}) missing");
            var errorNpc = NpcFactory.CreateErrorNpcObject(npcKey);
            _createdNpcDict.Add(errorNpc.NpcData.Key, errorNpc);
            return errorNpc;
        }

        var npc = NpcFactory.CreateNpcObject(new NpcCreationParameter()
        {
            NpcData = npcData,
            AnimationData = _animationData
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

    public void SetNpcPosition(List<Npc> npcList, bool isAnimating)
    {
        if (npcList == null || npcList.Count == 0) return;
        if (_slots == null || _slots.Count == 0) return;
        
        int npcIndex = 0;

        npcList = npcList.Where(x => x != null).ToList();

        for (int i = 0; i < _slots.Count; i++)
        {
            Npc npc = null;
            
            if (npcIndex >= npcList.Count) return;
            npc = npcList[npcIndex++];
            
            if (npc == null) return;
            
            Transform slot = _slots[i];
            Debug.Assert(slot);

            if (isAnimating)
            {
                npc.transform.DOMove(slot.position, _animationData.NpcMoveToSlotSpeed);
            }
            else
            {
                npc.transform.position = slot.position;
            }
        }
    }
}
