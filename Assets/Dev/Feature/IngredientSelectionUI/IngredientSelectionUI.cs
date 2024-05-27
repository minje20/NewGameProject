using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform[] _ingredientTransforms;
    [SerializeField] private Color32[] _ingredientColors;
    [SerializeField] private GameObject _ingredientPrefab;
    [SerializeField] private Transform _ingredientGroup;
    [SerializeField] private Sprite[] _ingredientSprites;
     private GameObject[] _ingredientPrefabs;

     private Vector2 _ingredientCaptureIndexRange = new Vector2(0, 4);

    private void Start()
    {
        _ingredientPrefabs = new GameObject[_ingredientTransforms.Length];

        for (int i = 0; i < _ingredientPrefabs.Length; i++)
        {
            _ingredientPrefabs[i] = Instantiate(_ingredientPrefab, 
                _ingredientTransforms[i].position,
                _ingredientTransforms[i].rotation,
                    _ingredientGroup);
            
            _ingredientPrefabs[i].transform.localScale = _ingredientTransforms[i].localScale;
            _ingredientPrefabs[i].GetComponent<Image>().color = _ingredientColors[i];
            _ingredientPrefabs[i].GetComponent<Image>().sprite = _ingredientSprites[i];
        }
    }

    public void TurnRight()
    {
        if ((int)_ingredientCaptureIndexRange.x == _ingredientPrefabs.Length - 3)
        {
            return;
        }
        _ingredientCaptureIndexRange.x++;
        _ingredientCaptureIndexRange.y++;
        
        for (int i = 0; i < _ingredientPrefabs.Length; i++)
        {
            if (_ingredientPrefabs[i] == null)
            {
                continue;
            }
            
            if (i == 0)
            {
                Destroy(_ingredientPrefabs[i]);
                continue;
            }

            _ingredientPrefabs[i].transform.DOMove(_ingredientTransforms[i - 1].position, 0.5f);
            _ingredientPrefabs[i].transform.DOScale(_ingredientTransforms[i - 1].localScale, 0.5f);
            _ingredientPrefabs[i].GetComponent<Image>().DOColor(_ingredientColors[i - 1], 0.5f);
            
            _ingredientPrefabs[i - 1] = _ingredientPrefabs[i];
            _ingredientPrefabs[i] = null;
            
            if (i == _ingredientPrefabs.Length - 1 && _ingredientCaptureIndexRange.y - i <= 0)
            {
                _ingredientPrefabs[i] = Instantiate(_ingredientPrefab,
                    _ingredientTransforms[i].position,
                    _ingredientTransforms[i].rotation,
                    _ingredientGroup);

                _ingredientPrefabs[i].GetComponent<Image>().sprite = _ingredientSprites[i + (int)_ingredientCaptureIndexRange.x];
                _ingredientPrefabs[i].transform.localScale = _ingredientTransforms[i].localScale;
            }
        }
    }
    
    public void TurnLeft()
    {
        if ((int)_ingredientCaptureIndexRange.x == -2)
        {
            return;
        }
        _ingredientCaptureIndexRange.x--;
        _ingredientCaptureIndexRange.y--;
        for (int i = _ingredientPrefabs.Length - 1; i >= 0; i--)
        {
            if (_ingredientPrefabs[i] == null)
            {
                continue;
            }
            
            if (i == _ingredientPrefabs.Length - 1)
            {
                Destroy(_ingredientPrefabs[i]);
                continue;
            }

            _ingredientPrefabs[i].transform.DOMove(_ingredientTransforms[i + 1].position, 0.5f);
            _ingredientPrefabs[i].transform.DOScale(_ingredientTransforms[i + 1].localScale, 0.5f);
            _ingredientPrefabs[i].GetComponent<Image>().DOColor(_ingredientColors[i + 1], 0.5f);
            
            _ingredientPrefabs[i + 1] = _ingredientPrefabs[i];

            _ingredientPrefabs[i] = null;
            if (i == 0 && _ingredientCaptureIndexRange.x + i >= 0)
            {
                _ingredientPrefabs[i] = Instantiate(_ingredientPrefab,
                    _ingredientTransforms[i].position,
                    _ingredientTransforms[i].rotation,
                    _ingredientGroup);
                
                _ingredientPrefabs[i].GetComponent<Image>().sprite = _ingredientSprites[i + (int)_ingredientCaptureIndexRange.x];
                _ingredientPrefabs[i].transform.localScale = _ingredientTransforms[i].localScale;
            }
        }
    }
}  