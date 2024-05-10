using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;


[System.Serializable]
public class AudioIndexEnumTableSet
{
    [SerializeField] internal AudioIndex _key;
    [SerializeField] internal AudioClip _clip;

    public AudioIndex Key => _key;
    public AudioClip Clip => _clip;
}

public class AudioIndexEnumTable : ScriptableObject
{
    [SerializeField] internal List<AudioIndexEnumTableSet> _table;

    public IReadOnlyList<AudioIndexEnumTableSet> Table => _table;
    
}
