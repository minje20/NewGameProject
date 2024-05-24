using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidRemover : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & _mask.value) == 0) return;
        
        Destroy(other.gameObject);
    }
}
