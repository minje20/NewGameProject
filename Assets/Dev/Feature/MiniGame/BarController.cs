using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class BarController : MonoBehaviour
{
    [field: SerializeField, InitializationField, MustBeAssigned]
    private Transform _slot;

    [SerializeField] private float _fadeinDuration = 2f;
    [SerializeField] private float _fadeoutDuration = 2f;

    private CocktailData _currentCocktailData;

    public CocktailData CurrentCocktailData
    {
        get => _currentCocktailData;
        private set
        {
            _currentCocktailData = value;
            if (value == false) return;
            
            _cocktailObject = value.ClonePrefab();
            _cocktailObject.gameObject.SetActive(false);
            _cocktailRenderer = _cocktailObject.GetComponent<SpriteRenderer>();
            _cocktailObject.transform.position = _slot.position;
        }
    }

    private RecipeData _currentRecipeData;
    public RecipeData CurrentRecipeData
    {
        get => _currentRecipeData;
        set
        {
            _currentRecipeData = value;
            CurrentCocktailData = value.Cocktail;
        }
    }

    private GameObject _cocktailObject;
    private SpriteRenderer _cocktailRenderer;

    private void Awake()
    {
        _slot.gameObject.SetActive(false);
    }

    public void ShowDrink()
    {
        _cocktailObject.gameObject.SetActive(true);
        
        _cocktailRenderer.color = Color.black;
        _cocktailRenderer.DOColor(Color.white, _fadeinDuration);
    }

    public void HideDrink()
    {
        _cocktailObject.gameObject.SetActive(false);
        _cocktailRenderer.color = Color.white;
        _cocktailRenderer.DOColor(Color.black, _fadeoutDuration).OnComplete(() =>
            _cocktailRenderer.gameObject.SetActive(false));
    }
}
