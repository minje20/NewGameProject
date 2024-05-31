using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyBox;
using Spine.Unity;
using UnityEngine;


[CreateAssetMenu(menuName = "IndieLINY/Npc/Data", fileName = "New npc data")]
public class NpcData: ScriptableObject
{

    [Serializable]
    public class SpriteKeyPair
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _key;
        [SerializeField] private Vector3 _offset;

        public Sprite Sprite => _sprite;
        public string Key => _key;

        public Vector3 Offset => _offset;
    }
    [Serializable]
    public class SpriteAniKeyPair
    {
        [SerializeField] private AnimationClip _clip;
        [SerializeField] private string _key;
        [SerializeField] private Vector3 _offset;

        public AnimationClip Clip => _clip;
        public string Key => _key;

        public Vector3 Offset => _offset;
    }

    [Serializable]
    public class SpineKeyPair
    {
        [Serializable]
        public class KeyPair
        {
            [SerializeField] private string _key;
            [SerializeField] private string _spineKey;

            public string Key => _key;
            public string SpineKey => _spineKey;
        }
        [SerializeField] private SkeletonDataAsset _asset;
        [SerializeField] private List<KeyPair> _containAnimationKeys;
        [SerializeField] private Vector3 _offset;

        public SkeletonDataAsset Asset => _asset;
        public Vector3 Offset => _offset;
        public List<KeyPair> ContainAnimationKeys => _containAnimationKeys;
    }
    
    [field: SerializeField]
    private string _key;

    [field: SerializeField, OverrideLabel("Npc 기본 스프라이트")]
    private Sprite _defaultSprite;

    [field: SerializeField]
    private List<SpineKeyPair> _spineAssets;
    
    [field: SerializeField]
    private List<SpriteKeyPair> _sprites;
    
    [field: SerializeField]
    private List<SpriteAniKeyPair> _spriteAnis;

    [field: SerializeField] 
    private AnimationClip _defaultAniClip;

    public string Key => _key;
    public List<SpriteKeyPair> Sprites => _sprites;
    public Sprite DefaultSprite => _defaultSprite;
    public List<SpineKeyPair> SpineAsset => _spineAssets;

    public List<SpriteAniKeyPair> SpriteAnis => _spriteAnis;

    public AnimationClip DefaultAniClip => _defaultAniClip;

    public static NpcData CreateErrorData(string key = "")
    {
        var data = ScriptableObject.CreateInstance<NpcData>();
        data._key = key;
        data._sprites = new ();
        data._defaultSprite = null;
        data._spineAssets = new();

        return data;
    }
}
