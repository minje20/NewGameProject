using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniGameControllerTester : MonoBehaviour
{
    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), InitializationField, MustBeAssigned]
    private MiniGameController _miniGameController;
    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), InitializationField, MustBeAssigned]
    private CameraController _cameraController;

    [field: SerializeField, DefinedValues(nameof( GetTypes))]
    private string _debugActionType;

    private string[] GetTypes()
        => new[] { "VisitNextCocktail", "VisitOnly", "CocktailOnly" };
    private void Start()
    {
        switch (_debugActionType)
        {
            case "VisitNextCocktail":
                _ = VisitNextCocktail();
                break;
            case "VisitOnly":
                _ =  VisitOnly();
                break;
            case "CocktailOnly":
                _ =  CocktailOnly();
                break;
            default:
                Debug.LogError("올바르지 않은 debug key: " + _debugActionType);
                break;
        }
    }

    private async UniTask VisitNextCocktail()
    {
        await _cameraController.DollyBehaviour.MoveAsync("Visitor");
        _ = _cameraController.DollyBehaviour.MoveAsync("MakingCocktail");

        _miniGameController.GameStart();
    }

    private async UniTask VisitOnly()
    {
        await _cameraController.DollyBehaviour.MoveAsync("Visitor");
    }

    private async UniTask CocktailOnly()
    {
        await _cameraController.DollyBehaviour.MoveAsync("MakingCocktail");
        
        _miniGameController.GameStart();
    }
}
