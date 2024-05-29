using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shaker : MonoBehaviour
{
    private List<GameObject> _iceObject = new();
    private List<GameObject> _liquidObject = new();

    public void SetEnableIce(bool value)
    {
        _iceObject.ForEach(x=>x.SetActive(value));
    }

    public void SetEnableLiquid(bool value)
    {
        _liquidObject.ForEach(x=>x.SetActive(value));
    }

    public void SetIceObject(List<GameObject> list)
    {
        if (list == null) return;

        list.ForEach(x =>
        {
            _iceObject.Add(x);
            x.transform.SetParent(transform);
        });
    }
    public void SetLiquidObject(List<GameObject> list)
    {
        if (list == null) return;
        
        list.ForEach(x =>
        {
            _liquidObject.Add(x);
            x.transform.SetParent(transform);
        });
    }

    private void __MiniGame_Reset__()
    {
        _iceObject.ForEach(x =>
        {
            if (x)
            {
                Destroy(x);
            }
        });
        _liquidObject.ForEach(x =>
        {
            if (x)
            {
                Destroy(x);
            }
        });
        
        _iceObject.Clear();
    }
}
