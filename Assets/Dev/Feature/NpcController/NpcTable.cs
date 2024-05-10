using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/Npc/Table", fileName = "New Table")]
public class NpcTable : ScriptableObject
{
    [field: SerializeField, Header("Npc 리스트"), DisplayInspector]
    private List<NpcData> _npcDataList;


    public IReadOnlyDictionary<string, NpcData> CreateTable()
    {
        var table = new Dictionary<string, NpcData>();
                
        _npcDataList?.ForEach(x=>table.Add(x.Key, x));
        return table;
    }
}
