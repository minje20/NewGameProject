using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidCellular : MonoBehaviour
{
    private Vector2Int _position;

    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;

            transform.position = new Vector3(
                _position.x * Size.x,
                _position.y * Size.y,
                0f
            );
        }
    }
    private Vector2 _size;

    public Vector2 Size
    {
        get => _size;
        set
        {
            _size = value;
            transform.localScale = _size;
        }
    }

    public Vector3 WorldPosition => transform.position;
    
    public Bounds GetBounds()
    {
        var bound = new Bounds(WorldPosition, Size);

        return bound;
    }

    public void Release()
    {
        GameObject.Destroy(gameObject);
    }
    
    public void Simulate()
    {
        
    }
    
}
