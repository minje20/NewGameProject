using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;

public class RecipeSummaryView : MonoBehaviour
{
    [SerializeField] 
    private Vector3 _openPivot;

    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private TMP_Text _text;

    public TMP_Text Text => _text;
    
    public bool IsClosed { get; private set; }

    private void Awake()
    {
        gameObject.SetActive(false);
        IsClosed = true;
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
        IsClosed = false;
    }
    public void Close()
    {
        gameObject.SetActive(false);
        IsClosed = true;
    }
}
