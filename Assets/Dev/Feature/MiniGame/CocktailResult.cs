using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CocktailResult : MonoBehaviour
{
    [field: SerializeField, InitializationField, MustBeAssigned]
    private Transform _pivot;
    
    private GameObject _cocktail;
    
    public void SetCocktail(CocktailData data)
    {
        if (_cocktail)
        {
            Destroy(_cocktail);
            _cocktail = null;
        }
        
        _cocktail = data.ClonePrefab();

        _cocktail.transform.position = _pivot.position;
    }
}
