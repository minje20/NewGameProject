using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/Npc/Data", fileName = "New npc data")]
public class NpcData: ScriptableObject
{
    [field: SerializeField]
    private string _key;

    [field: SerializeField, OverrideLabel("Npc 기본 스프라이트")]
    private Sprite _defaultSprite;
    
    [field: SerializeField, Header("Npc 스프라이트 키")]
    private List<Sprite> _sprites;
    
    [field: SerializeField, Foldout("게임오브젝트"), OverrideLabel("Transform의 Scale 값")]
    private Vector3 _scale = Vector3.one;

    public Vector3 Scale => _scale;

    public string Key => _key;
    public List<Sprite> Sprites => _sprites;
    public Sprite DefaultSprite => _defaultSprite;

    public static NpcData CreateErrorData(string key = "")
    {
        var data = ScriptableObject.CreateInstance<NpcData>();
        data._key = key;
        data._sprites = new List<Sprite>();
        data._defaultSprite = null;

        return data;
    }
}
