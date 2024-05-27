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
    [SerializeField] private RenderTexture _renderTexture;

    private CocktailData _currentCocktailData;

    public CocktailData CurrentCocktailData
    {
        get => _currentCocktailData;
        private set
        {
            if (_currentRecipeData == null) return;
            
            _currentCocktailData = value;
            if (value == false) return;

            if (_cocktailObject)
            {
                Destroy(_cocktailObject);
            }
            
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
            if (_currentRecipeData == value) return;

            if (value == null)
            {
                _currentRecipeData = null;
                return;
            }
            
            Context.Reset();
            _currentRecipeData = value;
            CurrentCocktailData = value.Cocktail;
        }
    }

    private GameObject _cocktailObject;
    private SpriteRenderer _cocktailRenderer;

    public MiniGameContext Context { get; private set; } = new();

    public float TotalScore { get; set; }

    private void Awake()
    {
        _slot.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _renderTexture.Release();
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

public class MeasureItem
{
    public MiniMeasurementInfo Info;
    public EMiniGameScore Score;
    public bool IsEnd;
}

public class MiniGameContext
{
    public int CurrentIceCount { get; set; }
    public int IceMaxCount { get; set; }
    public bool IsIceEnd { get; set; }
    public EMiniGameScore IceScore { get; set; }
    
    public Dictionary<DrinkData, MeasureItem> MeasuredDrinkTable { get; private set; } = new();
    
    public bool IsShakeEnd { get; set; }
    public EMiniGameScore ShakeScore { get; set; }
 
    public void Reset()
    {
        MeasuredDrinkTable.Clear();
        
        CurrentIceCount = 0;
        IceMaxCount = 0;
        IsIceEnd = false;
        IceScore = EMiniGameScore.Bad;

        IsShakeEnd = false;
        ShakeScore = EMiniGameScore.Bad;
    }
}