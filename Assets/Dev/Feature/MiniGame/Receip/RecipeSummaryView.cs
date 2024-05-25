using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class RecipeSummaryView : MonoBehaviour
{
    [SerializeField] 
    private Vector3 _openPivot;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    [ButtonMethod]
    private void SetOpenPosition()
    {
        if (Application.isPlaying) return;
        
        _openPivot = transform.position;
    }

    public void Open()
    {
        transform.position = _openPivot;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
