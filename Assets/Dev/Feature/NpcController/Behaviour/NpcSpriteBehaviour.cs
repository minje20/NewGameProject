using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcSpriteBehaviour : BaseNpcBehaviour
{
    private SpriteRenderer _renderer;

    public SpriteRenderer Renderer => _renderer;

    public override void Stop()
    {
        gameObject.SetActive(false);
    }

    protected override void Init()
    {
        _renderer = gameObject.AddComponent<SpriteRenderer>();
        _renderer.sortingLayerName = "Npc";
    }
    
    public override bool PlayAnimation(string key)
    {
        if (_renderer == false) return false;

        var pair = Data.Sprites.Find(x => x.Key == key);
        if (pair == null) return false;

        gameObject.SetActive(true);
        
        _renderer.sprite = pair.Sprite;
        transform.localPosition = pair.Offset;
        
        return true;
    }

    public void SetDefault()
    {
        gameObject.SetActive(true);
        _renderer.sprite = Data.DefaultSprite;
    }
}
