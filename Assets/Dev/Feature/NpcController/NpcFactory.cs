using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NpcCreationParameter
{
    public NpcData NpcData;
    public NpcAnimationData AnimationData;
}

public static class NpcFactory
{
    public static Npc CreateNpcObject(NpcCreationParameter parameter)
    {
        var npcData = parameter.NpcData;
        if (npcData == null) return null;

        var npc = CreateCommon(ref parameter);

        return npc;
    }
    
    public static Npc CreateErrorNpcObject(string key = "")
    {
        var parameter = new NpcCreationParameter()
        {
            AnimationData = NpcAnimationData.CreateErrorData(),
            NpcData = NpcData.CreateErrorData(key)
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
        npc.Init(parameter);
        

        return npc;
    }

    public static void DestroyNpc(Npc npc)
    {
        GameObject.Destroy(npc.gameObject);
    }
}
