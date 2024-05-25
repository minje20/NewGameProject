using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CocktailResult : MonoBehaviour
{
    [field: SerializeField, InitializationField, MustBeAssigned]
    private Transform _pivot;

    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), InitializationField, MustBeAssigned]
    private BarController _controller;

    [field: SerializeField, InitializationField, MustBeAssigned]
    private GameObject _pannel;
    
    private GameObject _cocktail;
    
    private void Update()
    {
        if (_pannel.activeSelf == false) return;
        
        if (_controller == false || _controller.CurrentCocktailData == false) return;
        
        if (_cocktail)
        {
            Destroy(_cocktail);
        }
        
        _cocktail = _controller.CurrentCocktailData.ClonePrefab();

        _cocktail.transform.position = _pivot.position;
    }
}
