using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class NpcFactory
{
    public static Npc CreateNpcObject(NpcCreationParameter parameter)
    {
        var npcData = parameter.NpcData;
        if (npcData == null) return null;

        var npc = CreateCommon(ref parameter);

        return npc;
    }
    
    public static Npc CreateErrorNpcObject(NpcSlot slot, string key = "")
    {
        var parameter = new NpcCreationParameter()
        {
            AnimationData = NpcAnimationData.CreateErrorData(),
            NpcData = NpcData.CreateErrorData(key),
            Slot = slot
        };

        var npc = CreateCommon(ref parameter);

        return npc;
    }

    private static Npc CreateCommon(ref NpcCreationParameter parameter)
    {
        GameObject obj = new GameObject(parameter.NpcData.Key);

        var spr = obj.AddComponent<SpriteRenderer>();
        spr.sprite = parameter.NpcData.DefaultSprite;
        spr.color = parameter.AnimationData.FadeoutColor;
        spr.sortingLayerName = "Npc";

        var npc = obj.AddComponent<Npc>();
        npc.gameObject.transform.localScale = parameter.NpcData.Scale;
        npc.transform.position = parameter.Slot.Transform.position;
        
        npc.Init(parameter);
        

        return npc;
    }

    public static void DestroyNpc(Npc npc)
    {
        GameObject.Destroy(npc.gameObject);
    }
}
